using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        public Cart()
        {
            Id = 1;
        }
        public int Id { get; private set; }
        //public decimal Total { get; private set; }
        private readonly List<CartItem> _cartItem = new List<CartItem>();

        public IReadOnlyCollection<CartItem> CartItems => _cartItem.AsReadOnly();

        public bool UpdateCart(int productId, int quantity)
        {
            var existigItem = _cartItem.FirstOrDefault(i => i.ProductId == productId);
            if(existigItem != null)
            {
                existigItem.UpdateQuantity(quantity);
                if(existigItem.Quantity == 0)
                {
                    _cartItem.Remove(existigItem);
                    return true;
                }
            }
            else
            {
                if(quantity > 0)
                {
                    _cartItem.Add(new CartItem(productId, quantity));
                    return true;
                }
            }
            return false;
        }
   
    }
}
