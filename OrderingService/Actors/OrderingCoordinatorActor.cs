using Akka.Actor;
using Messages.Commands;
using Messages.Events;


namespace OrderingService.Actors
{
    public class OrderingCoordinatorActor: ReceiveActor
    {
        private readonly IActorRef ProductCatalogActor;
        private readonly IActorRef OrderingActor;
        private readonly IActorRef CartCoordinatorActor;

        public OrderingCoordinatorActor(IActorRef productCatalogActor, IActorRef orderingActor, IActorRef cartCoordinatorActor)
        {
            ProductCatalogActor = productCatalogActor;
            OrderingActor = orderingActor;
            CartCoordinatorActor = cartCoordinatorActor;

            ReceiveAsync<CreateOrder>(async cmd =>
            {
                var reservedCount = 0;
                List<ReserveProductResult> reserveResults = new List<ReserveProductResult>();

                foreach (var item in cmd.Cart.CartItems)
                {
                    var inventoryStatus = await ProductCatalogActor.Ask<InventoryStatus>(new LookupProduct(item.ProductId));
                    if (inventoryStatus == null || inventoryStatus.AvailableQuantity == 0)
                        OrderingActor.Tell(new OrderFailed("Product not found"));

                    var reserveProductResult = await ProductCatalogActor.Ask<ReserveProductResult>(new ReserveProduct(item.ProductId, item.Quantity, inventoryStatus.Version));
                    reserveResults.Add(reserveProductResult);
                    if (reserveProductResult is ReserveProductSuccess reserveProductSuccess)
                    {
                        reservedCount++;
                    }
                }

                if (reservedCount == cmd.Cart.CartItems.Count)
                {
                    await OrderingActor.Ask<OrderResult>(cmd).PipeTo(Self, Sender);                    
                }
                else
                {
                    foreach (var item in reserveResults.OfType<ReserveProductSuccess>())
                    {
                        ProductCatalogActor.Tell(new ReleaseProductReservation(item.ProductId, item.Quantity));
                    }
                    CartCoordinatorActor.Tell(new ClearCart());
                    
                    if (reserveResults.Any(x => x is ReserveProductFailed))
                    {
                        Sender.Tell(new OrderFailed("Version mismatch occurred while reserving the products."));
                    }
                    else
                    {
                        Sender.Tell(new OrderFailed("Not all products could be reserved."));
                    }
                }
            });

            ReceiveAsync<OrderSuccess>(async orderSuccess =>
            {
                Sender.Tell(new OrderSuccess());                
                CartCoordinatorActor.Tell(new ClearCart());
                OrderingActor.Tell(new ClearOrder());
            });
            ReceiveAsync<OrderFailed>(async orderFailed =>
            {
                CartCoordinatorActor.Tell(new ClearCart());
                OrderingActor.Tell(new ClearOrder());
                Sender.Tell(new OrderFailed(orderFailed.Message));
            });
            ReceiveAsync<OrderCanceled>(async orderCancelled =>
            {
                Sender.Tell(new OrderCanceled());
                OrderingActor.Tell(new ClearOrder());
                CartCoordinatorActor.Tell(new ClearCart());
            });
            ReceiveAsync<CancelOrder>(async cmd =>
            {
                var releaseTasks = cmd.Order.OrderItems.Select(item =>
                    ProductCatalogActor.Ask<ReleaseProductReservation>(new ReleaseProductReservation(item.ProductId, item.Quantity))).ToList();
                await Task.WhenAll(releaseTasks);

                await OrderingActor.Ask<OrderResult>(cmd).PipeTo(Self, Sender);
            });

        }
    }
}
