using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SURE_Store_API.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using SURE_Store_API.DTOs;

namespace SURE_Store_API.Controllers
{
    
    [ApiController]  // Indicates this is an API controller
    [Route("api/[controller]")]  // Route template: api/cart
    [Authorize]  // Require authentication for all cart operations
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;  // Store cart service reference
        }
        [HttpGet]  // HTTP GET endpoint
        public async Task<ActionResult<CartResponse>> GetCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _cartService.GetCartAsync(userId);
            return Ok(response);
        }
        [HttpPost]  // HTTP POST endpoint
        public async Task<ActionResult<CartResponse>> AddToCart(AddToCartRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _cartService.AddToCartAsync(userId, request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpPut("{itemId}")]  // HTTP PUT endpoint with item ID parameter
        public async Task<ActionResult<CartResponse>> UpdateCartItem(int itemId, UpdateCartItemRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _cartService.UpdateCartItemAsync(userId, itemId, request);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpDelete("{itemId}")]  // HTTP DELETE endpoint with item ID parameter
        public async Task<ActionResult<CartResponse>> RemoveFromCart(int itemId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _cartService.RemoveFromCartAsync(userId, itemId);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
        [HttpDelete]  // HTTP DELETE endpoint
        public async Task<ActionResult<CartResponse>> ClearCart()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _cartService.ClearCartAsync(userId);
            return Ok(response);
        }


    }

}