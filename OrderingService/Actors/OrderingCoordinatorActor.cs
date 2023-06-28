using Akka.Actor;
using Domain.Entities;
using Messages.Commands;
using Messages.Events;


namespace OrderingService.Actors
{
    public class OrderingCoordinatorActor: ReceiveActor
    {
        private readonly IActorRef ProductCatalogActor;
        private readonly Dictionary<string, IActorRef> OrderingActors = new Dictionary<string, IActorRef>();
        private readonly IActorRef CartCoordinatorActor;

        public OrderingCoordinatorActor(IActorRef productCatalogActor, IActorRef cartCoordinatorActor)
        {
            ProductCatalogActor = productCatalogActor;
            CartCoordinatorActor = cartCoordinatorActor;

            ReceiveAsync<CreateOrder>(async cmd =>
            {
                var reservedCount = 0;
                List<ReserveProductResult> reserveResults = new List<ReserveProductResult>();
                string orderingActorGuid = Guid.NewGuid().ToString();
                IActorRef orderingActor = Context.ActorOf(Props.Create(() => new OrderingActor(orderingActorGuid)), nameof(OrderingActor) + "-"  + orderingActorGuid);
                OrderingActors.Add(orderingActorGuid, orderingActor);
                foreach (var item in cmd.Cart.CartItems)
                {
                    var inventoryStatus = await ProductCatalogActor.Ask<InventoryStatus>(new LookupProduct(item.ProductId));
                    if (inventoryStatus == null || inventoryStatus.AvailableQuantity == 0)
                        orderingActor.Tell(new OrderFailed(orderingActorGuid, "Product not found"));

                    var reserveProductResult = await ProductCatalogActor.Ask<ReserveProductResult>(new ReserveProduct(item.ProductId, item.Quantity, inventoryStatus.Version));
                    reserveResults.Add(reserveProductResult);
                    if (reserveProductResult is ReserveProductSuccess reserveProductSuccess)
                    {
                        reservedCount++;
                    }
                }

                if (reservedCount == cmd.Cart.CartItems.Count)
                {
                    await orderingActor.Ask<OrderResult>(cmd).PipeTo(Self, Sender);                    
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
                Sender.Tell(new OrderSuccess(orderSuccess.OrderId));                
                CartCoordinatorActor.Tell(new ClearCart());
                IActorRef orderingActor = OrderingActors[orderSuccess.OrderId];
                orderingActor.Tell(new ClearOrder());
            });
            ReceiveAsync<OrderFailed>(async orderFailed =>
            {
                IActorRef orderingActor = OrderingActors[orderFailed.OrderId];
                orderingActor.Tell(new ClearOrder());
                CartCoordinatorActor.Tell(new ClearCart());
                Sender.Tell(new OrderFailed(orderFailed.Message));
            });
            ReceiveAsync<OrderCanceled>(async orderCancelled =>
            {
                IActorRef orderingActor = OrderingActors[orderCancelled.OrderId];
                orderingActor.Tell(new ClearOrder());

                Sender.Tell(new OrderCanceled(orderCancelled.OrderId));
                CartCoordinatorActor.Tell(new ClearCart());
            });
            ReceiveAsync<CancelOrder>(async cmd =>
            {
                IActorRef orderingActor = OrderingActors[cmd.Order.Id];

                var releaseTasks = cmd.Order.OrderItems.Select(item =>
                    ProductCatalogActor.Ask<ReleaseProductReservation>(new ReleaseProductReservation(item.ProductId, item.Quantity))).ToList();
                await Task.WhenAll(releaseTasks);

                await orderingActor.Ask<OrderResult>(cmd).PipeTo(Self, Sender);
            });

        }   

    }
}
