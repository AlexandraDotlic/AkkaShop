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
    }
}
