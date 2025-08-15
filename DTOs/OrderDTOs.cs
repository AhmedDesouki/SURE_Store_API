using SURE_Store_API.DTOs.Order;
using SURE_Store_API.Models;
using System.ComponentModel.DataAnnotations;

namespace SURE_Store_API.DTOs
{
    public class PlaceOrderRequest {
        [Required]  // Validation: field is mandatory
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Shipping address must be at least 10 characters")]  // Validation: 10-500 characters
        public string ShippingAddress { get; set; } = string.Empty;

    }
    public class OrderItemDto {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Priced { get; set; }
        public decimal TotalPrice { get; set; }




    }


    public class OrderDTOs
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public String ShippingAddress { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public int TotalItems { get; set; }
    }
    public class AdminOrderDto : OrderDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }
    public class OrderListResponse {
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
    public class CreateOrderRequest
    {
        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Shipping address must be at least 10 characters")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
    public class UpdateOrderStatusRequest {
        [Required]
        public OrderStatus Status { get; set; }
    }


}
