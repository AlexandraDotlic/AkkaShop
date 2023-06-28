using Akka.Actor;
using Domain.Entities;
using Messages.Commands;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService
{
    public class ProductCatalogSvc : IProductCatalogService
    {
        private IActorRef ProductCatalogActor;

        public ProductCatalogSvc(IActorRef productCatalogActor)
        {
            ProductCatalogActor = productCatalogActor;
        }

        public async Task<InventoryStatus> AddProduct(Product product)
        {
            var result = await ProductCatalogActor.Ask(new AddProduct(product));
            return (InventoryStatus)result;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var result = await ProductCatalogActor.Ask<GetAllProducts>(new GetAllProducts());
            if (result != null)
                return result.ProductList;
            else
                return null;

        }

        public async Task<bool> LookupProduct(int productId)
        {
            var result = await ProductCatalogActor.Ask(new LookupProduct(productId));
            if(result != null) 
            {
                if (result is InventoryStatus)
                    return true;
                else if (result is ProductNotFound)
                    return false;
            }
            return false;
        }

        public async Task<InventoryStatus> UpdateInventory(int productId, int quantity, int version)
        {
            var result = await ProductCatalogActor.Ask(new UpdateInventory(productId, quantity, version));
            return (InventoryStatus)result;
        }
    }
}
