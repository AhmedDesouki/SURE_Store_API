using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SURE_Store_API.Services;
using System.Security.Claims;

namespace SURE_Store_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("User ID not found in token.");
            return int.Parse(userIdClaim.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _cartService.GetCart(GetUserId());
            var total = cart?.CartItems.Sum(item => item.Quantity * item.Product.Price) ?? 0;

            return Ok(new
            {
                Items = cart?.CartItems.Select(item => new
                {
                    item.Id,
                    item.ProductId,
                    ProductName = item.Product.Name,
                    item.Quantity,
                    item.Product.Price,
                    SubTotal = item.Quantity * item.Product.Price
                }),
                Total = total
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest request)
        {
            await _cartService.AddToCart(GetUserId(), request.ProductId, request.Quantity);
            return Ok(new { Message = "Product added to cart successfully" });
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateQuantity(int itemId, [FromBody] UpdateCartQuantityRequest request)
        {
            await _cartService.UpdateQuantity(GetUserId(), itemId, request.Quantity);
            return Ok(new { Message = "Cart item updated successfully" });
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            await _cartService.RemoveFromCart(GetUserId(), itemId);
            return Ok(new { Message = "Cart item removed successfully" });
        }
    }

    public class CartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartQuantityRequest
    {
        public int Quantity { get; set; }
    }
}
