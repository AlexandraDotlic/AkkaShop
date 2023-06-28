using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class InventoryStatus
    {
        public int ProductId { get; }
        public int AvailableQuantity { get; }
        public int Version { get; }

        public InventoryStatus(int productId, int availableQuantity, int version)
        {
            ProductId = productId;
            AvailableQuantity = availableQuantity;
            Version = version;
        }
}
}
