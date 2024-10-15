using Application.Interfaces;
using Application.Models.CartDtos;
using Domain.Enums;
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _cartService.GetCartsByUserIdAsync(userId);
            return cart != null ? Ok(cart) : NotFound();
        }

        [HttpPost("/add-product/{productId}")]
        public async Task<IActionResult> AddProductToCart(int cartId,int productId, int quantity)
        {
            // Obtener el id del usuario logueado
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID is not valid.");
            }

            // Validar el valor de quantity
            if (quantity <= 0)
            {
                return BadRequest("La cantidad debe ser mayor que 0.");
            }

            // Agregar el producto al carrito
            await _cartService.AddProductToCartAsync(userId, productId, quantity);

            return Ok("Producto agregado al carrito.");
        }

        [HttpDelete("/remove-product/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(int cartId, int productId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Id de usuario no encontrado.");
            }
           
            await _cartService.RemoveProductFromCartAsync(userId, cartId, productId);
            return NoContent();
        }

        [HttpPost("/clear")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Id de usuario no encontrado.");
            }
            await _cartService.ClearCartAsync(userId, cartId);
            return NoContent();
        }

        [HttpGet("{userId}/total-price")]
        public async Task<IActionResult> CalculateTotalPrice(int userId, int cartId)
        {
            var totalPrice = await _cartService.CalculateTotalPriceAsync(userId, cartId);
            return Ok(totalPrice);
        }

        [HttpPost("{cartId}/pay")]
        public async Task<IActionResult> PayCart(int cartId, [FromBody] PaymentRequest paymentRequest)
        {
            // Obtener el id del usuario logueado desde los claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar si el userIdClaim es válido
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("Id de usuario no encontrado.");
            }

            // Validar que el método de pago sea válido
            if (!Enum.IsDefined(typeof(TypePayment), paymentRequest.TypePayment))
            {
                return BadRequest("Método de pago inválido.");
            }

            // Llamar al servicio para realizar el pago del carrito con el método de pago seleccionado
            try
            {
                await _cartService.PayCartAsync(userId, cartId, paymentRequest.TypePayment);
                return Ok("Carrito pagado con éxito.");
            }
            catch (Exception ex)
            {
                // Manejar el error si el carrito no se encuentra
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("paid")]
        public async Task<IActionResult> GetPaidCarts()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Verificar si userIdClaim es nulo o no se puede convertir a int
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID is not valid.");
            }

            var paidCarts = await _cartService.GetPaidCartsByUserIdAsync(userId);
            return Ok(paidCarts);
        }
    }
}
