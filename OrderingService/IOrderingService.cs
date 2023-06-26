using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingService
{
    public interface IOrderingService
    {
        Task<OrderCreateResult> CreateOrder(Cart cart);
        Task CancelOrder(Order order);
    }
}
