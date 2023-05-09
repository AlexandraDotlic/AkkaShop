using Domain.Entities;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService
{
    public interface IProductCatalogService
    {
        Task<bool> LookupProduct(int productId);
        Task<InventoryStatus> UpdateInventory(int productId, int quantity);
        Task<List<Product>> GetAllProducts();
        Task<InventoryStatus> AddProduct(Product product);

    }
}
