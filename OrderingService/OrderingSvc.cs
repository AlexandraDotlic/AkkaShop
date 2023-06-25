using Akka.Actor;
using Domain.Entities;
using OrderingService.Messages.Commands;
using OrderingService.Messages.Events;

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

        public async Task CancelOrder(Guid orderId)
        {
           OrderingActor.Tell(new CancelOrder(orderId));         
        }

        public async Task<OrderCreateResult> CreateOrder(Cart cart)
        {            
            var response = await OrderingCoordinatorActor.Ask(new CreateOrder(cart));
            if (response is CreateOrderFailed)
                return new OrderCreateResult { Success = false, Message = "failed to create order" };

            return new OrderCreateResult { Success = true };

        }
    }
}
