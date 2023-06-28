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
        Task<OrderResult> CreateOrder(Cart cart);
        Task<OrderResult> CancelOrder(Order order);
    }
}
