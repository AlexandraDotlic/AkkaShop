using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class ReserveProductFailed : ReserveProductResult
    {
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }

        public ReserveProductFailed(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
