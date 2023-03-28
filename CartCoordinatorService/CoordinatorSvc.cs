using Akka.Actor;
using Akka.Util;
using CartService;
using Domain.Entities;
using Messages.Commands;
using Messages.Events;

namespace CartCoordinatorService
{
    public class CoordinatorSvc : ICoordinatorService
    {
        private readonly IActorRef CartCoordinatorActor;
        private readonly ICartService CartService;
        private readonly IActorRef OrderingCoordinatorActor;

        public CoordinatorSvc(IActorRef cartCoordinatorActor, ICartService cartService, IActorRef orderingCoordinatorActor)
        {
            CartCoordinatorActor = cartCoordinatorActor;
            CartService = cartService;
            OrderingCoordinatorActor = orderingCoordinatorActor;
        }

        public async Task<CartUpdateResult> AddToCart(int productId, int quantity, decimal price, int? cartId = null)
        {

            var response = await CartCoordinatorActor.Ask(new AddToCart(cartId, productId, quantity, price));
            if (response is CartUpdateSuccess)
                return new CartUpdateResult { Success = true };
            else if (response is CartUpdateFailed)
                return new CartUpdateResult { Success = false, ErrorMessage = ((CartUpdateFailed)response).ErrorMessage };
            else
                return new CartUpdateResult { Success = false, ErrorMessage = "Add to Cart failed" };

        }

        public async Task<CartUpdateResult> RemoveFromCart(int cartId, int productId, int quantity)
        {
            var response = await CartCoordinatorActor.Ask(new RemoveFromCart(cartId, productId, quantity));
            if (response is CartUpdateSuccess)
                return new CartUpdateResult { Success = true };
            else if(response is CartUpdateFailed)
                return new CartUpdateResult { Success = false, ErrorMessage = ((CartUpdateFailed)response).ErrorMessage };
            else
                return new CartUpdateResult { Success = false, ErrorMessage = "Remove from Cart failed" };
        }

        public async Task CancelOrder(Guid orderId)
        {
            OrderingCoordinatorActor.Tell(new CancelOrder(orderId));
        }

        public async Task<OrderCreateResult> CreateOrder(int cartId)
        {
            var cart = await CartService.GetCart(cartId);
            var response = await OrderingCoordinatorActor.Ask(new CreateOrder(cart));
            if (response is CreateOrderFailed)
                return new OrderCreateResult { Success = false, Message = "failed to create order" };

            return new OrderCreateResult { Success = true, OrderId = ((OrderSuccess)response).OrderId.ToString()};

        }
    }
}
