using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Commands
{
    public class ReserveProduct
    {
        public int ProductId { get; private set; }
        public int Quantity { get; private set; }
        public int Version { get; set; }

        public ReserveProduct(int productId, int quantity, int version)
        {
            ProductId = productId;
            Quantity = quantity;
            Version = version;
        }
    }
}
