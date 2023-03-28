using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Events
{
    public class CartUpdated
    {
        public CartUpdated(int productId, int quantity, decimal? price = null)
        {
            ProductId = productId;
            Quantity = quantity;
            Price = price;
        }

        public int ProductId { get; }
        public int Quantity { get; }
        public decimal? Price { get; set; }

    }

}
