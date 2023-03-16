using Akka.Actor;
using CartService.Actors;
using Domain.Entities;
using Messages.Commands;
using Newtonsoft.Json;

namespace CartService
{
    public class CartSvc: ICartService
    {
        private readonly IActorRef CartActor;

        public CartSvc(IActorRef cartActor)
        {
            this.CartActor = cartActor;
        }

        //nisam sigurna kako po Id-ju da vratim i da li mi to treba ovde
        public async Task<Cart> GetCart(int cartId)
        {
            var result = await CartActor.Ask(new GetCart());
            return (Cart)result;
        }

    }

}
