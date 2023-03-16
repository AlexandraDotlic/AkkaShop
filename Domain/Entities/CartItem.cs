using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CartItem
    {
        public CartItem()
        {

        }
        public CartItem(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
        }

        public int Id { get; private set; }

        public int ProductId { get; private set; }

        public int Quantity { get; private set; }

        internal void IncreaseQuantity(int quantity = 1)
        {
            Quantity += quantity;
        }
        public void DecreaseQuantity()
        {
            if (Quantity > 0)
            {
                Quantity -= 1;
            }
        }
    }
}
