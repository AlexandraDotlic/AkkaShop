using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Commands
{
    public class AddToCart
    {
        public int? CartId { get; set; }
        public int ProductId { get; }
        public int Quantity { get; }
        public decimal Price { get; set; }
        public AddToCart(int productId, int quantity, decimal price)
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
        public AddToCart(int? cartId, int productId, int quantity, decimal price)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }
    }
}
