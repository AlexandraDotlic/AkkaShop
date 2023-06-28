using Akka.Actor;
using Akka.Persistence;
using Domain.Entities;
using Messages.Commands;
using Messages.Events;

namespace OrderingService.Actors
{
    public class OrderingActor : UntypedPersistentActor
    {
        public override string PersistenceId => nameof(OrderingActor) + "-" + Id;
        private Order Order;
        private string Id;
        public OrderingActor(string id)
        {
            Id = id;
        }
        protected override void OnCommand(object message)
        {
            switch (message)
            {
                case CreateOrder createOrder:
                    if (Order != null)
                    {
                        Sender.Tell(new OrderFailed(Order.Id, "An order is already being processed."));
                    }
                    else
                    {
                        Order = new Order(OrderStatus.Created, createOrder.Cart, Id);
                        Persist(new OrderCreated(Order), _ =>
                        {
                            Sender.Tell(new OrderSuccess(Order.Id));
                        });
                    }                 
                    break;
                case CancelOrder cancelOrder:
                    if (Order == null)
                    {
                        Sender.Tell(new OrderFailed("There is no order to cancel."));
                    }
                    else
                    {
                        Order.CancelOrder();
                        Persist(new OrderCanceled(Order.Id), _ =>
                        {
                            Sender.Tell(new OrderSuccess(Order.Id));
                        });
                    }                   
                    break;
                case ClearOrder clearOrder:
                    Persist(new OrderCleared(), _ =>
                    {
                        Sender.Tell(new Messages.Events.OrderResult(Order.Id));
                        Order = null;
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
                case OrderCanceled _:
                    Order.CancelOrder();
                    break;
                case OrderCleared _:
                    Order = null;
                    break;
            }
        }
    }
}
