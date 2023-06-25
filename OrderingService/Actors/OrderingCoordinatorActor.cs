using Akka.Actor;
using OrderingService.Messages.Commands;
using OrderingService.Messages.Events;

namespace OrderingService.Actors
{
    public class OrderingCoordinatorActor: ReceiveActor
    {
        private readonly IActorRef ProductCatalogActor;
        private readonly IActorRef OrderingActor;

        public OrderingCoordinatorActor( IActorRef productCatalogActor, IActorRef orderingActor)
        {
            ProductCatalogActor = productCatalogActor;
            OrderingActor = orderingActor;

            
            ReceiveAsync<CreateOrder>(async cmd =>
            {
                
                int errorCount = 0;
                foreach (var item in cmd.Cart.CartItems)
                {
                    var inventoryStatus = await ProductCatalogActor.Ask<InventoryStatus>(new LookupProduct(item.ProductId));
                    if (inventoryStatus.AvailableQuantity < item.Quantity)
                    {
                        errorCount++;
                    }
                }
                if (errorCount > 0)
                    Sender.Tell(new CreateOrderFailed());
                else
                {
                    OrderingActor.Tell(cmd);
                    Sender.Tell(new OrderSuccess());
                }

            });
            ReceiveAsync<CancelOrder>(async cmd =>
            { 
                Sender.Tell(cmd);

            });

        }
    }
}
