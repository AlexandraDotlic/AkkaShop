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
        private readonly IActorRef OrderingCoordinatorActor;


        public OrderingSvc(IActorRef orderingActor, IActorRef orderingCoordinatorActor)
        {
            OrderingActor = orderingActor;
            OrderingCoordinatorActor = orderingCoordinatorActor;
        }

        public async Task CancelOrder(Order order)
        {
           OrderingActor.Tell(new CancelOrder(order));         
        }

        public async Task<OrderCreateResult> CreateOrder(Cart cart)
        {            
            var response = await OrderingCoordinatorActor.Ask(new CreateOrder(cart));
            if (response is OrderFailed)
                return new OrderCreateResult { Success = false, Message = "failed to create order" };

            return new OrderCreateResult { Success = true };

        }
    }
}
