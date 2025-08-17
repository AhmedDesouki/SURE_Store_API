using SURE_Store_API.DTOs;
using SURE_Store_API.Models;

namespace SURE_Store_API.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(string userId, CreateOrderRequest request);
        Task<OrderDto?> GetOrderByIdAsync(int orderId, string userId);
        Task<OrderListResponse> GetUserOrdersAsync(string userId, int page = 1, int pageSize = 10);
        Task<OrderListResponse> GetAllOrdersAsync(int page = 1, int pageSize = 10, OrderStatus? status = null);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderRequest request);
        Task<bool> CancelOrderAsync(int orderId, string userId);
        Task<bool> ProcessPaymentAsync(int orderId, string paymentMethod);
        Task<OrderStatisticsDto> GetOrderStatisticsAsync();
    }
     public class OrderStatisticsDto
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }

    }
}
