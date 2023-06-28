using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Commands
{
    public class UpdateInventory
    {
        public UpdateInventory(int productId, int quantity, int productVersion)
        {
            ProductId = productId;
            Quantity = quantity;
            ProductVersion = productVersion;
        }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ProductVersion { get; set; }
    }
}
