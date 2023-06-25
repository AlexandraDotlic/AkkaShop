using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Messages.Commands
{
    public class RemoveFromCart
    {
        public int ProductId { get; }
        public int Quantity { get; }

        public RemoveFromCart( int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}

