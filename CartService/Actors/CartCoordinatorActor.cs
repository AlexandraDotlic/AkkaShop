using Akka.Actor;
using Messages.Commands;
using Messages.Events;

namespace CartService.Actors
{
    public class CartCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef CartActor;
        private readonly IActorRef ProductCatalogActor;
        public CartCoordinatorActor(IActorRef cartActor, IActorRef productCatalogActor)
        {
            CartActor = cartActor;
            ProductCatalogActor = productCatalogActor;

            ReceiveAsync<AddToCart>(async cmd =>
            {
                try
                {
                    var inventoryStatus = await productCatalogActor.Ask<InventoryStatus>(new LookupProduct(cmd.ProductId));

                    if (inventoryStatus.AvailableQuantity >= cmd.Quantity)
                    {
                        CartActor.Tell(cmd);
                        ProductCatalogActor.Tell(new UpdateInventory(cmd.ProductId, -cmd.Quantity));
                        Sender.Tell(new CartUpdateSuccess());
                    }
                    else
                    {
                        Sender.Tell(new CartUpdateFailed($"Not enough inventory for {cmd.ProductId}"));
                    }
                }
                catch (Exception ex)
                {
                    Sender.Tell(new CartUpdateFailed($"An error occurred while updating the cart: {ex.Message}"));
                }
            });

            Receive<RemoveFromCart>(cmd =>
            {
                CartActor.Tell(cmd);
                ProductCatalogActor.Tell(new UpdateInventory(cmd.ProductId, cmd.Quantity));
                Sender.Tell(new CartUpdateSuccess());
            });
        }
    }
}
