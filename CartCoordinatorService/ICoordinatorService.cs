using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartCoordinatorService
{
    public interface ICoordinatorService
    {
        Task<CartUpdateResult> AddToCart(int productId, int quantity, decimal price, int? cartId=null);
        Task<CartUpdateResult> RemoveFromCart(int cartId, int productId, int quantity);
        Task<OrderCreateResult> CreateOrder(int cartId);
    }
}
