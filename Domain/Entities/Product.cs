using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public Product()
        {

        }
        public Product(int id, string title, decimal price, int inventory = 1)
        {  
            Id = id;
            Title = title;
            Price = price;
            Quantity = inventory;
            ReservedQuantity = 0;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public int ReservedQuantity { get; private set; }

        /// <summary>
        /// update the inventory levels of the product, based on the quantity being added or removed from the inventory.
        /// </summary>
        /// <param name="quantity"></param>
        public void ChangeQuantity(int quantity)
        {
            if (Quantity + quantity >= 0)
                Quantity += quantity;
        }
        /// <summary>
        /// Update the price of the product
        /// </summary>
        /// <param name="price"></param>
        public void UpdatePrice(decimal price)
        {
            Price = price;
        }
        /// <summary>
        ///  check the availability of the product, based on the current inventory levels.
        /// </summary>
        /// <returns></returns>
        public bool CheckAvailability()
        {
            return Quantity > 0;
        }

        public void IncreaseReservedQuantity(int quantity)
        {
           ReservedQuantity+= quantity;
        }

        public void DecreaseReservedQuantity(int quantity)
        {
            ReservedQuantity -= quantity;
        }
    }
}
