using Application.Interfaces;
using Application.Models.CartDtos;
using Domain.Enums;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet("/get-carts-by-user")]
        public async Task<IActionResult> GetCart()
        {
            // Obtener el id del usuario logueado
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new NotFoundException("User ID is not valid.");
            }

            var cart = await _cartService.GetCartsByUserIdAsync(userId);
            return cart != null ? Ok(cart) : throw new NotFoundException("User ID is not valid."); ;
        }

        [Authorize]
        [HttpPost("/add-product/{productId}")]
        public async Task<IActionResult> AddProductToCart(int productId, int quantity)
        {
            // Obtener el id del usuario logueado
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new NotFoundException("User ID is not valid.");
            }

            // Validar el valor de quantity
            if (quantity <= 0)
            {
                throw new BadRequestException("The amount must be greater than 0.");
            }

            // Agregar el producto al carrito
            await _cartService.AddProductToCartAsync(userId, productId, quantity);

            return Ok("Product added to cart.");
        }

        [Authorize]
        [HttpDelete("/remove-product/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(int productId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new NotFoundException("User ID not found.");
            }
           
            await _cartService.RemoveProductFromCartAsync(userId, productId);
            return NoContent();
        }

        [Authorize]
        [HttpPost("/clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new NotFoundException("User ID not found.");
            }
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("/cart/total-price")]
        public async Task<IActionResult> CalculateTotalPrice( int cartId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new NotFoundException("User ID not found.");
            }

            var totalPrice = await _cartService.CalculateTotalPriceAsync(userId, cartId);
            return Ok(totalPrice);
        }

        [Authorize]
        [HttpPost("{cartId}/pay")]
        public async Task<IActionResult> PayCart(int cartId, [FromBody] PaymentRequest paymentRequest)
        {
            // Obtener el id del usuario logueado desde los claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar si el userIdClaim es válido
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedException("User ID not found.");
            }

            // Validar que el método de pago sea válido
            if (!Enum.IsDefined(typeof(TypePayment), paymentRequest.TypePayment))
            {
                throw new BadRequestException("Invalid payment method.");
            }

            // Llamar al servicio para realizar el pago del carrito con el método de pago seleccionado
             await _cartService.PayCartAsync(userId, cartId, paymentRequest.TypePayment);
             return Ok("Cart successfully paid.");
            
        }

        [Authorize]
        [HttpGet("paid")]
        public async Task<IActionResult> GetPaidCarts()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new NotFoundException("User ID is not valid.");
            }

            var paidCarts = await _cartService.GetPaidCartsByUserIdAsync(userId);
            return Ok(paidCarts);
        }
    }
}
