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
                foreach (var item in cmd.Cart.CartItems)
                {
                    var reserveProductResult = await ProductCatalogActor.Ask<ReserveProductResult>(new ReserveProduct(item.ProductId, item.Quantity));

                    if (reserveProductResult is ReserveProductSuccess)                   
                        reservedCount++;                    
                }

                if (reservedCount == cmd.Cart.CartItems.Count)
                {
                    await OrderingActor.Ask<OrderResult>(cmd).PipeTo(Self, Sender);                    
                }
                else
                {
                    foreach (var item in cmd.Cart.CartItems)
                    {
                        ProductCatalogActor.Tell(new ReleaseProductReservation(item.ProductId, item.Quantity));
                    }
                    CartCoordinatorActor.Tell(new ClearCart());
                    Sender.Tell(new OrderFailed("Not all products could be reserved."));
                }
            });

            ReceiveAsync<OrderSuccess>(async orderSuccess =>
            {
                Sender.Tell(new OrderSuccess());
                CartCoordinatorActor.Tell(new ClearCart());
            });
            ReceiveAsync<OrderFailed>(async orderFailed =>
            {
                CartCoordinatorActor.Tell(new ClearCart());
                Sender.Tell(new OrderFailed(orderFailed.Message));
            });
            ReceiveAsync<OrderCanceled>(async orderCancelled =>
            {
                Sender.Tell(new OrderCanceled());
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
