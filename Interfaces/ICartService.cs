
using SURE_Store_API.Models;

namespace OnlineStoreAPI.Services
{
    public interface ICartService
    {
        Task<Cart> GetCart(int userId);
        Task<Cart> AddToCart(int userId, int productId, int quantity);
        Task<Cart> UpdateQuantity(int userId, int cartItemId, int quantity);
        Task<Cart> RemoveFromCart(int userId, int cartItemId);
    }
}
