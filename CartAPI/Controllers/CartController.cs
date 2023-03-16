using CartCoordinatorService;
using CartService;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService CartService;
        private readonly ICartCoordinatorService CartCoordinatorService;

        public CartController(ICartService cartService)
        {
            CartService = cartService;
        }

        [HttpGet("{cartId}")]
        public async Task<ActionResult<Cart>> GetCart(int cartId)
        {
            var cart = await CartService.GetCart(cartId); //todo:
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPost("{cartId}/add")]
        public async Task<ActionResult> AddItemToCart(int cartId, [FromBody] AddToCartMessage message)
        {
            await CartCoordinatorService.AddToCart(cartId, message.ProductId, message.Quantity);
            return Ok();
        }

        [HttpPost("{cartId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int itemId, int quantity)
        {
            await CartCoordinatorService.RemoveFromCart(cartId, itemId, quantity);
            return Ok();
        }
    }
}
