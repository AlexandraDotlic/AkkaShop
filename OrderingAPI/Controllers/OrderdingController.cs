using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using OrderingService;

namespace OrderingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderdingController : ControllerBase
    {
        private readonly IOrderingService OrderingService;
        public OrderdingController(IOrderingService orderingService)
        {
            OrderingService = orderingService;
        }
        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody]CartDTO cart)
        {
            List<CartItem> cartItems = new List<CartItem>();
            foreach (var item in cart.CartItems)
            {
                CartItem cartItem = new CartItem(item.ProductId, item.Quantity, item.Price);
                cartItems.Add(cartItem);
            }
            Cart c = new Cart(cart.Id, cartItems);
            var result = await OrderingService.CreateOrder(c);
            return Ok(result);
        }
    }
}
