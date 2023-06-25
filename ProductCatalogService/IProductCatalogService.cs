using Domain.Entities;
using ProductCatalogService.Messages.Events;

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
