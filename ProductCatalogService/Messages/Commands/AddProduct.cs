using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Messages.Commands
{
    public class AddProduct
    {
        public Product Product { get; }

        public AddProduct(Product product)
        {
            Product = product;
        }
    }
}
