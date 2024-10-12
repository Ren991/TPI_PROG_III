using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            return cart != null ? Ok(cart) : NotFound();
        }

        [HttpPost("{userId}/add-product/{productId}")]
        public async Task<IActionResult> AddProductToCart(int userId, int productId, int quantity)
        {
            await _cartService.AddProductToCartAsync(userId, productId, quantity);
            return NoContent();
        }

        [HttpDelete("{userId}/remove-product/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(int userId, int productId)
        {
            await _cartService.RemoveProductFromCartAsync(userId, productId);
            return NoContent();
        }

        [HttpPost("{userId}/clear")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpGet("{userId}/total-price")]
        public async Task<IActionResult> CalculateTotalPrice(int userId)
        {
            var totalPrice = await _cartService.CalculateTotalPriceAsync(userId);
            return Ok(totalPrice);
        }
    }
}
