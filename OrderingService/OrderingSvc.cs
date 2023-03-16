using Akka.Actor;
using Domain.Entities;
using Messages.Commands;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingService
{
    public class OrderingSvc : IOrderingService
    {
        private readonly IActorRef OrderingActor;

        public OrderingSvc(IActorRef orderingActor)
        {
            OrderingActor = orderingActor;
        }

        public async Task CancelOrder(Guid orderId)
        {
           OrderingActor.Tell(new CancelOrder(orderId));         
        }

        public async Task<Order> CreateOrder(Cart cart)
        {
            Order order = new Order(OrderStatus.Created, cart.Id);
            var response = await OrderingActor.Ask(new CreateOrder(order));
            if (response != null)
            {
                if (response is OrderCreated)
                {
                    return order;
                }
            }           
            return null;
        }
    }
}
