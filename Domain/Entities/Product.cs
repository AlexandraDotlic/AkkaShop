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
        public Product(string title, decimal price, int inventory)
        {
            Title = title;
            Price = price;
            Inventory = inventory;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public int Inventory { get; private set; }

        /// <summary>
        /// update the inventory levels of the product, based on the quantity being added or removed from the inventory.
        /// </summary>
        /// <param name="quantity"></param>
        public void ChangeQuantity(int quantity)
        {
            if (Inventory + quantity >= 0)
                Inventory += quantity;
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
            return Inventory > 0;
        }
    }
}
