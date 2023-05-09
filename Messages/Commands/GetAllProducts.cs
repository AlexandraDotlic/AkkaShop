using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Commands
{
    public class GetAllProducts
    {
        public GetAllProducts()
        {
        }

        public GetAllProducts(List<Product> productList)
        {
            ProductList = productList;
        }

        public List<Product> ProductList { get; private set; }

    }
}
