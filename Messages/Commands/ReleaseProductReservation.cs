using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Commands
{
    public class ReleaseProductReservation
    {
        public ReleaseProductReservation(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
    }
}
