﻿using Akka.Actor;
using Akka.Persistence;
using Domain.Entities;
using OrderingService.Messages.Commands;
using OrderingService.Messages.Events;

namespace OrderingService.Actors
{
    public class OrderingActor : UntypedPersistentActor
    {
        public override string PersistenceId => nameof(OrderingActor);

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
                    Order = new Order(OrderStatus.Created, createOrder.Cart);
                    Persist(new OrderCreated(Order), _ =>
                    {
                        Sender.Tell(new OrderSuccess());
                    });
                    break;
                case CancelOrder cancelOrder:
                    if (Order == null)
                    {
                        Sender.Tell(new OrderFailed("There is no order to cancel."));
                        return;
                    }
                    Order.CancelOrder();
                    Persist(new OrderCanceled(), _ =>
                    {
                        Sender.Tell(new OrderSuccess());
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
                    Order.CancelOrder();
                    break;
            }
        }
    }
}
