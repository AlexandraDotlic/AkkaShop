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
            var r = new Random();
            Id = r.Next();
        }
        public int Id { get; private set; }
        private readonly List<CartItem> _cartItem = new List<CartItem>();

        public IReadOnlyCollection<CartItem> CartItems => _cartItem.AsReadOnly();

        public bool UpdateCart(int productId, int quantity, decimal? price = null)
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
                if(quantity > 0 && price != null)
                {
                    _cartItem.Add(new CartItem(productId, quantity, price.Value));
                    return true;
                }
            }
            return false;
        }

        public decimal CalculateTotalCost()
        {
            decimal totalCost = 0;
            foreach (var item in CartItems)
            {
                totalCost += item.Quantity * item.Price;
            }
            return totalCost;
        }

        public Cart(int id, List<CartItem> cartItem)
        {
            Id = id;
            _cartItem = cartItem;
        }
    }
}
