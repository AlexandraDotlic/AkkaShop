using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Messages.Commands
{
    public class LookupProduct
    {
        public int ProductId { get; }

        public LookupProduct(int productId)
        {
            ProductId = productId;
        }
    }
}
