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
    public class ProductCatalogActor : ReceivePersistentActor
    {

        private readonly List<Product> Products = new List<Product>();

        public ProductCatalogActor()
        {

            Recover<Product>(product =>
            {
                Products.Add(product);
            });

            Command<GetAllProducts>(getAllProducts =>
            {
                Sender.Tell(new GetAllProducts(Products));
            });

            Command<LookupProduct>(lookupProduct =>
            {
                var product = Products.Find(p => lookupProduct.ProductId == p.Id);
                if (product.CheckAvailability())
                {
                    Sender.Tell(new InventoryStatus(lookupProduct.ProductId, product.Quantity));
                }
                else
                {
                    Sender.Tell(new ProductNotFound(lookupProduct.ProductId));
                }
            });

            Command<UpdateInventory>(updateInv =>
            {
                var product = Products.FirstOrDefault(p => updateInv.ProductId == p.Id);
                if (product != null)
                {
                    product.ChangeQuantity(updateInv.Quantity);
                    Sender.Tell(new InventoryStatus(updateInv.ProductId, product.Quantity));
                    Persist(product, p =>
                    {
                        Products.Remove(product);
                        Products.Add(p);
                    });
                }
                else
                {
                    Sender.Tell(new ProductNotFound(updateInv.ProductId));
                }
            });

            Command<AddProduct>(addProduct =>
            {
                var product = Products.FirstOrDefault(p => addProduct.Product.Id == p.Id);
                if (product != null)
                {
                    product.ChangeQuantity(addProduct.Product.Quantity);
                    Sender.Tell(new InventoryStatus(addProduct.Product.Id, product.Quantity));
                    Persist(product, p =>
                    {
                        Products.Remove(product);
                        Products.Add(p);
                    });
                }
                else
                {
                    Sender.Tell(new InventoryStatus(addProduct.Product.Id, addProduct.Product.Quantity));
                    Persist(addProduct.Product, product =>
                    {
                        Products.Add(product);
                    });
                }
            });
        }


        public override string PersistenceId => nameof(ProductCatalogActor);
    }
}
