using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingService.Messages.Events
{
    public class OrderCreated
    {
        public OrderCreated(Order order)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }
}
