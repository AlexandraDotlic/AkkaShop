using Akka.Actor;
using CartService.Messages.Commands;
using CartService.Messages.Events;
using Domain.Entities;

namespace CartService
{
    public class CartSvc: ICartService
    {
        private readonly IActorRef CartActor;
        private readonly IActorRef CartCoordinatorActor;

        public CartSvc(IActorRef cartActor, IActorRef cartCoordinatorActor)
        {
            CartActor = cartActor;
            CartCoordinatorActor = cartCoordinatorActor;
        }

        //nisam sigurna kako po Id-ju da vratim i da li mi to treba ovde
        public async Task<Cart> GetCart()
        {
            var result = await CartActor.Ask(new GetCart());
            return (Cart)result;
        }

        public async Task<CartUpdateResult> AddToCart(int productId, int quantity, decimal price)
        {

            var response = await CartCoordinatorActor.Ask(new AddToCart(productId, quantity, price));
            if (response is CartUpdateSuccess)
                return new CartUpdateResult { Success = true };
            else if (response is CartUpdateFailed)
                return new CartUpdateResult { Success = false, ErrorMessage = ((CartUpdateFailed)response).ErrorMessage };
            else
                return new CartUpdateResult { Success = false, ErrorMessage = "Add to Cart failed" };

        }

        public async Task<CartUpdateResult> RemoveFromCart( int productId, int quantity)
        {
            var response = await CartCoordinatorActor.Ask(new RemoveFromCart(productId, quantity));
            if (response is CartUpdateSuccess)
                return new CartUpdateResult { Success = true };
            else if (response is CartUpdateFailed)
                return new CartUpdateResult { Success = false, ErrorMessage = ((CartUpdateFailed)response).ErrorMessage };
            else
                return new CartUpdateResult { Success = false, ErrorMessage = "Remove from Cart failed" };
        }

    }

}
