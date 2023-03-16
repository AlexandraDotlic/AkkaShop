using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartCoordinatorService
{
    public interface ICartCoordinatorService
    {
        Task<CartUpdateResult> AddToCart(int productId, int quantity, int? cartId=null);
        Task<CartUpdateResult> RemoveFromCart(int cartId, int productId, int quantity);
    }
}
