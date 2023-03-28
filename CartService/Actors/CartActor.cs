﻿using Akka.Actor;
using Akka.Persistence;
using Messages.Events;
using Domain.Entities;
using Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Actors
{
    /// <summary>
    /// This actor handles two types of messages - AddToCart and RemoveFromCart.
    /// When it receives an AddToCart message, it persists a CartUpdated event with the item and quantity, and then updates its state to reflect the added items. 
    /// Similarly, when it receives a RemoveFromCart message, it persists a CartUpdated event with the item and negative quantity, and 
    /// then updates its state to reflect the removed items. The state is stored in a private property Cart.
    /// </summary>
    public class CartActor : UntypedPersistentActor
    {
        public override string PersistenceId => nameof(CartActor);
        private Cart Cart = new Cart();

        protected override void OnCommand(object message)
        {
            switch (message)
            {
                //todo: getCart
                case GetCart getCart:                   
                        var cart = Cart;
                        Sender.Tell(cart);
                        break;                                       
                case AddToCart addToCart:
                    Persist(new CartUpdated(addToCart.ProductId, addToCart.Quantity), _ =>
                    {
                        if(addToCart.CartId == null)
                        {
                            Cart = new Cart();
                        }
                        Cart.UpdateCart(addToCart.ProductId, addToCart.Quantity);
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
                    Cart.UpdateCart(cartUpdated.ProductId, cartUpdated.Quantity);
                    break;
            }
        }


    }
}
