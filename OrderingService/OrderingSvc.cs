using Akka.Actor;
using Azure;
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
        private readonly IActorRef OrderingCoordinatorActor;


        public OrderingSvc(IActorRef orderingCoordinatorActor)
        {
            OrderingCoordinatorActor = orderingCoordinatorActor;
        }

        public async Task<OrderResult> CancelOrder(Order order)
        {
            Messages.Events.OrderResult response = await OrderingCoordinatorActor.Ask<Messages.Events.OrderResult>(new CancelOrder(order));
            if (response is OrderFailed)
                return new OrderResult { Success = false, Message = ((OrderFailed)response).Message };

            return new OrderResult { Success = true, Message = "Order canceled.", OrderId = response.OrderId };

        }

        public async Task<OrderResult> CreateOrder(Cart cart)
        {
            Messages.Events.OrderResult response = await OrderingCoordinatorActor.Ask<Messages.Events.OrderResult>(new CreateOrder(cart));
            if (response is OrderFailed)
                return new OrderResult { Success = false, Message = ((OrderFailed)response).Message };

            return new OrderResult { Success = true, Message = "Order created." , OrderId = response.OrderId};

        }
    }
}
