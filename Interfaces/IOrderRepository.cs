using SURE_Store_API.Models;

namespace SURE_Store_API.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderWithItemsAsync(int orderId, string userId);
    }
}
