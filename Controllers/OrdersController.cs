using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SURE_Store_API.DTOs;
using SURE_Store_API.Services;
using SURE_Store_API.Models;
using System.Security.Claims;

namespace SURE_Store_API.Controllers
{
    
    // Controller for handling order-related operations
    // Provides endpoints for order creation, management, and tracking
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

       
        // Creates a new order from cart items
        
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var userId = GetUserId();
                var order = await _orderService.CreateOrderAsync(userId, request);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while creating the order" });
            }
        }

        
        // Retrieves a specific order by ID
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var userId = GetUserId();
            var order = await _orderService.GetOrderByIdAsync(id, userId);

            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }

        
        // Retrieves all orders for the current user
       
        [HttpGet]
        public async Task<ActionResult<OrderListResponse>> GetUserOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = GetUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId, page, pageSize);
            return Ok(orders);
        }

        
        // Retrieves all orders (admin only)
        
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderListResponse>> GetAllOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] OrderStatus? status = null)
        {
            var orders = await _orderService.GetAllOrdersAsync(page, pageSize, status);
            return Ok(orders);
        }

        
        // Updates the status of an order (admin only)
        
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(int id, UpdateOrderRequest request)
        {
            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(id, request);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating the order" });
            }
        }

        
        // Cancels an order
        
        
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> CancelOrder(int id)
        {
            try
            {
                var userId = GetUserId();
                var success = await _orderService.CancelOrderAsync(id, userId);

                if (!success)
                    return NotFound(new { message = "Order not found" });

                return Ok(new { message = "Order cancelled successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while cancelling the order" });
            }
        }

        
       // Processes payment for an order
        
        [HttpPost("{id}/payment")]
        public async Task<ActionResult> ProcessPayment(int id, [FromBody] PaymentRequest request)
        {
            try
            {
                var success = await _orderService.ProcessPaymentAsync(id, request.PaymentMethod);

                if (!success)
                    return NotFound(new { message = "Order not found" });

                return Ok(new { message = "Payment processed successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while processing payment" });
            }
        }

        
        // Gets order statistics for admin dashboard
        
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderStatisticsDto>> GetOrderStatistics()
        {
            var statistics = await _orderService.GetOrderStatisticsAsync();
            return Ok(statistics);
        }

        
        // Gets order summary for the current user
        
        [HttpGet("summary")]
        public async Task<ActionResult<List<OrderSummaryDto>>> GetOrderSummary()
        {
            var userId = GetUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId, 1, 5); // Get last 5 orders

            var summaries = orders.Orders.Select(o => new OrderSummaryDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                ItemCount = o.OrderItems.Count
            }).ToList();

            return Ok(summaries);
        }

       
        // Extracts user ID from JWT token claims
       
        private string GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("User ID not found in token");

            return userIdClaim.Value;
        }
    }

  
    // Payment request DTO
   
    public class PaymentRequest
    {
        public string PaymentMethod { get; set; } = string.Empty;
    }
}