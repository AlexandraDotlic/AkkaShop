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

        [HttpGet]
        public async Task<ActionResult<Cart>> GetCart()
        {
            var cart = await CartService.GetCart(); //todo:
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpPost]
        public async Task<ActionResult> AddItemToCart( [FromBody] AddToCartMessage message)
        {

            await CartService.AddToCart(message.ProductId, message.Quantity, message.Price);
            return Ok();
        }

        [HttpPost("{cartId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromCart(int itemId, int quantity)
        {
            await CartService.RemoveFromCart(itemId, quantity);
            return Ok();
        }
    }
}
