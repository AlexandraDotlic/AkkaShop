using Akka.Actor;
using Akka.Persistence;
using Domain.Entities;
using Messages.Commands;
using Messages.Events;

namespace OrderingService.Actors
{
    public class OrderingActor : UntypedPersistentActor
    {
        public override string PersistenceId => "order";

        private Order Order;

        protected override void OnCommand(object message)
        {
            switch (message)
            {
                case CreateOrder createOrder:
                    if (Order != null)
                    {
                        Sender.Tell(new OrderFailed("An order is already being processed."));
                        return;
                    }

                    Persist(new OrderCreated(createOrder.Order), _ =>
                    {
                        Order = createOrder.Order;
                        Sender.Tell(new OrderSuccess(Order.Id));
                    });
                    break;
                case CancelOrder cancelOrder:
                    if (Order == null)
                    {
                        Sender.Tell(new OrderFailed("There is no order to cancel."));
                        return;
                    }

                    Persist(new OrderCanceled(Order.Id), _ =>
                    {
                        Order = null;
                        Sender.Tell(new OrderSuccess(Order.Id));
                    });
                    break;
            }
        }

        protected override void OnRecover(object message)
        {
            switch (message)
            {
                case OrderCreated orderCreated:

                    Order = orderCreated.Order;
                    break;
                case OrderCanceled orderCancelled:
                    Order = null;
                    break;
            }
        }
    }
}
