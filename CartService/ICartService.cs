using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService
{
    public interface ICartService
    {
        Task<Cart> GetCart();
        Task<ResultCartUpdate> AddToCart(int productId, int quantity, decimal price);
        Task<ResultCartUpdate> RemoveFromCart( int productId, int quantity);

    }
}
