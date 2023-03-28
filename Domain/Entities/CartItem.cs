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

        public int ProductId { get; private set; }

        public int Quantity { get; private set; }

        internal void UpdateQuantity(int quantity = 1)
        {
            if((Quantity + quantity) >= 0)
            {
                Quantity += quantity;
            }
            else
            {
                throw new Exception("To many items");
            }
            
        }
    
    }
}
