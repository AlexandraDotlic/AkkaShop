using Akka.Actor;
using Akka.Persistence;
using Domain.Entities;
using Messages.Commands;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogService.Actors
{
    public class ProductCatalogActor : ReceiveActor
    {

        private readonly List<Product> Products; //todo: populate

        public ProductCatalogActor()
        {
            Products = new List<Product>(); //treba populisati ovu listu
            var p1 = new Product("test", 10, 100); //test
            Products.Add(p1);
            //todo: get products from somwhere
            Receive<LookupProduct>(lookupProduct =>
            {
                var product = Products.Find(p => lookupProduct.ProductId == p.Id);
                if (product.CheckAvailability())
                {
                    Sender.Tell(new InventoryStatus(lookupProduct.ProductId, product.Inventory));
                }
                else
                {
                    Sender.Tell(new ProductNotFound(lookupProduct.ProductId));
                }
            });

            Receive<UpdateInventory>(updateInv =>
            {
                var product = Products.FirstOrDefault(p => updateInv.ProductId == p.Id);
                if (product != null)
                {
                    product.ChangeQuantity(updateInv.Quantity);
                    Sender.Tell(new InventoryStatus(updateInv.ProductId, product.Inventory));
                }
                else
                {
                    Sender.Tell(new ProductNotFound(updateInv.ProductId));
                }
            });


        }
    }
}
