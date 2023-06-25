using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Messages.Events
{
    public class ProductNotFound
    {
        public ProductNotFound(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; set; }

    }
}
