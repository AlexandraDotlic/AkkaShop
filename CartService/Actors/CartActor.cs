using Akka.Actor;
using Akka.Persistence;
using CartService.Messages.Commands;
using CartService.Messages.Events;
using Domain.Entities;

namespace CartService.Actors
{
    public class CartActor : UntypedPersistentActor
    {
        public override string PersistenceId => nameof(CartActor);
        private Cart Cart;

        protected override void OnCommand(object message)
        {
            switch (message)
            {
                case GetCart getCart:
                        if(Cart == null)
                        {
                            Cart = new Cart();
                        }                       
                        Sender.Tell(Cart);
                        break;                                       
                case AddToCart addToCart:
                    Persist(new CartUpdated(addToCart.ProductId, addToCart.Quantity, addToCart.Price), _ =>
                    {
                        if(Cart == null)
                        {
                            Cart = new Cart();
                        }
                        Cart.UpdateCart(addToCart.ProductId, addToCart.Quantity, addToCart.Price);
                        Sender.Tell(new CartUpdateSuccess());
                    });
                    break;
                case RemoveFromCart removeFromCart:
                    Persist(new CartUpdated(removeFromCart.ProductId, removeFromCart.Quantity), _ =>
                    {
                        Cart.UpdateCart(removeFromCart.ProductId, -removeFromCart.Quantity);
                        Sender.Tell(new CartUpdateSuccess());
                    });
                    break;
            }
        }
        protected override void OnRecover(object message)
        {
            switch (message)
            {
                case CartUpdated cartUpdated:
                    if (Cart == null)
                    {
                        Cart = new Cart();
                    }

                    if (cartUpdated.Price.HasValue)
                    {
                        Cart.UpdateCart(cartUpdated.ProductId, cartUpdated.Quantity, cartUpdated.Price.Value);
                    }
                    else
                    {
                        Cart.UpdateCart(cartUpdated.ProductId, -cartUpdated.Quantity);
                    }
                    break;
            }
        }
    }
}

