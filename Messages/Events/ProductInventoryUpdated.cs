using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class ProductInventoryUpdated
    {
        public Guid ProductId { get; }
        public int Quantity { get; }

        public ProductInventoryUpdated(Guid productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
