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

        public AddToCart(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
        public AddToCart(int? cartId, int productId, int quantity)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
