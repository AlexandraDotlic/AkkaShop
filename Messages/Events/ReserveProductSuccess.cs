using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class ReserveProductSuccess : ReserveProductResult
    {
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }

        public ReserveProductSuccess(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
