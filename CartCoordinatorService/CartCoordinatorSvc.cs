using Akka.Actor;
using Messages.Commands;
using Messages.Events;

namespace CartCoordinatorService
{
    public class CartCoordinatorSvc : ICartCoordinatorService
    {
        private readonly IActorRef CartCoordinatorActor;

        public CartCoordinatorSvc(IActorRef cartCoordinatorActor)
        {
            CartCoordinatorActor = cartCoordinatorActor;
        }

        public async Task<CartUpdateResult> AddToCart(int productId, int quantity, int? cartId = null)
        {

            var response = await CartCoordinatorActor.Ask(new AddToCart(cartId, productId, quantity));
            if (response is CartUpdateSuccess)
                return new CartUpdateResult { Success = true };
            else if (response is CartUpdateFailed)
                return new CartUpdateResult { Success = false, ErrorMessage = ((CartUpdateFailed)response).ErrorMessage };
            else
                return new CartUpdateResult { Success = false, ErrorMessage = "Add to Cart failed" };

        }

        public async Task<CartUpdateResult> RemoveFromCart(int cartId, int productId, int quantity)
        {
            var response = await CartCoordinatorActor.Ask(new RemoveFromCart(cartId, productId, quantity));
            if (response is CartUpdateSuccess)
                return new CartUpdateResult { Success = true };
            else if(response is CartUpdateFailed)
                return new CartUpdateResult { Success = false, ErrorMessage = ((CartUpdateFailed)response).ErrorMessage };
            else
                return new CartUpdateResult { Success = false, ErrorMessage = "Remove from Cart failed" };
        }
    }
}
