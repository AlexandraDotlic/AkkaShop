using CartService;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService CartService;

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

            await CartService.AddToCart(message.ProductId, message.Quantity, message.Price, cartId);
            return Ok();
        }

        [HttpPost("{cartId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int itemId, int quantity)
        {
            await CartService.RemoveFromCart(cartId, itemId, quantity);
            return Ok();
        }
    }
}
