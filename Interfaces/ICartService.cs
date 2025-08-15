using SURE_Store_API.DTOs.Cart;

namespace SURE_Store_API.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetUserCartAsync(string userId);
        Task<CartDto> AddToCartAsync(string userId, int productId, int quantity);
        Task<CartDto> UpdateCartItemAsync(string userId, int cartItemId, int quantity);
        Task<CartDto> RemoveFromCartAsync(string userId, int cartItemId);
        Task<CartDto> ClearCartAsync(string userId);
    }
}
