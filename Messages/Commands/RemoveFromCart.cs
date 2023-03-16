using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Commands
{
    public class RemoveFromCart
    {
        public int CartId { get; set; }
        public int ProductId { get; }
        public int Quantity { get; }

        public RemoveFromCart(int cartId, int productId, int quantity)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}

