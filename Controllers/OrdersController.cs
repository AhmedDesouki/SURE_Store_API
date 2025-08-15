using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SURE_Store_API.DTOs;
using SURE_Store_API.DTOs.Order;
using SURE_Store_API.Services;
using System.Security.Claims;

namespace SURE_Store_API.Controllers
{
    [ApiController]  // Indicates this is an API controller
    [Route("api/[controller]")]  // Route template: api/orders
    [Authorize]  // Require authentication for all order operations
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;  // Store order service reference
        }
        [HttpGet]  // HTTP GET endpoint
        public async Task<ActionResult<OrderListResponse>> GetOrders(
           [FromQuery] int page = 1,           // Page number from query string
           [FromQuery] int pageSize = 10)      // Page size from query string
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var response = await _orderService.GetOrdersAsync(userId, page, pageSize);
            return Ok(response);
        }
        [HttpGet("{id}")]  // HTTP GET endpoint with ID parameter
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var order = await _orderService.GetOrderByIdAsync(id, userId);
            if (order == null)
            {
                return NotFound();  // Return 404 if order not found
            }

            return Ok(order);
        }
        [HttpPost]  // HTTP POST endpoint
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(userId, request);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPut("{id}/status")]  // HTTP PUT endpoint for status update
        [Authorize(Roles = "Admin")]  // Require admin role
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(int id, UpdateOrderStatusRequest request)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(id, request);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpDelete("{id}")]  // HTTP DELETE endpoint
        public async Task<ActionResult> CancelOrder(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var success = await _orderService.CancelOrderAsync(id, userId);
                if (!success)
                {
                    return NotFound();  // Return 404 if order not found
                }

                return NoContent();  // Return 204 for successful cancellation
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        }
}
