using Akka.Actor;
using Akka.Persistence;
using Domain.Entities;
using Messages.Commands;
using Messages.Events;

namespace ProductCatalogService.Actors
{
    public class ProductCatalogActor : ReceivePersistentActor
    {
        private Dictionary<int, Product> Products = new Dictionary<int, Product>();
        public ProductCatalogActor()
        {
            Recover<Product>(product =>
            {                
                Products[product.Id] = product;
            });
            Command<ReserveProduct>(reserveProduct =>
            {
                if (Products.TryGetValue(reserveProduct.ProductId, out var product))
                {
                    if (product.Quantity >= reserveProduct.Quantity)
                    {
                        product.ChangeQuantity(-reserveProduct.Quantity);
                        product.IncreaseReservedQuantity(reserveProduct.Quantity);
                        Sender.Tell(new ReserveProductSuccess(reserveProduct.ProductId, reserveProduct.Quantity));
                        Persist(product, _ =>
                        {
                            Products[reserveProduct.ProductId] = product;
                        });
                    }
                    else
                    {
                        Sender.Tell(new ReserveProductFailed(reserveProduct.ProductId, reserveProduct.Quantity));
                    }
                }
                else
                {
                    Sender.Tell(new ProductNotFound(reserveProduct.ProductId));
                }
            });

            Command<ReleaseProductReservation>(releaseProductReservation =>
            {
                if (Products.TryGetValue(releaseProductReservation.ProductId, out var product))
                {
                    product.ChangeQuantity(releaseProductReservation.Quantity);
                    product.DecreaseReservedQuantity(releaseProductReservation.Quantity);
                    Sender.Tell(new ReleaseProductReservationSuccess(releaseProductReservation.ProductId, releaseProductReservation.Quantity));
                    Persist(product, _ =>
                    {
                        Products[releaseProductReservation.ProductId] = product; 
                    });
                }
                else
                {
                    Sender.Tell(new ProductNotFound(releaseProductReservation.ProductId));
                }
            });

            Command<GetAllProducts>(getAllProducts =>
            {
                Sender.Tell(new GetAllProducts(Products.Values.ToList()));
            });

            Command<LookupProduct>(lookupProduct =>
            {
                if (Products.TryGetValue(lookupProduct.ProductId, out var product))
                {
                    if (product.CheckAvailability())
                    {
                        Sender.Tell(new InventoryStatus(lookupProduct.ProductId, product.Quantity));
                    }
                    else
                    {
                        Sender.Tell(new ProductNotFound(lookupProduct.ProductId));
                    }
                }
                else
                {
                    Sender.Tell(new ProductNotFound(lookupProduct.ProductId));
                }              
            });

            Command<UpdateInventory>(updateInv =>
            {
                if (Products.TryGetValue(updateInv.ProductId, out var product))
                {
                    product.ChangeQuantity(updateInv.Quantity);
                    Sender.Tell(new InventoryStatus(updateInv.ProductId, product.Quantity));
                    Persist(product, _ =>
                    {
                        Products[updateInv.ProductId] = product;
                    });
                }
                else
                {
                    Sender.Tell(new ProductNotFound(updateInv.ProductId));
                }               
            });

            Command<AddProduct>(addProduct =>
            {
                if (Products.TryGetValue(addProduct.Product.Id, out var product))
                {
                    product.ChangeQuantity(addProduct.Product.Quantity);
                    Sender.Tell(new InventoryStatus(addProduct.Product.Id, product.Quantity));
                    Persist(product, _ =>
                    {
                        Products[addProduct.Product.Id] = product;
                    });
                }
                else
                {
                    Products.Add(addProduct.Product.Id, addProduct.Product);
                    Sender.Tell(new InventoryStatus(addProduct.Product.Id, addProduct.Product.Quantity));
                    Persist(addProduct, _ =>
                    {
                        Products[addProduct.Product.Id] = addProduct.Product;
                    });
                }               
            });
        }
        public override string PersistenceId => nameof(ProductCatalogActor);
    }
}
