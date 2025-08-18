using SURE_Store_API.Models;
using System.Threading.Tasks;

namespace SURE_Store_API.Interfaces
{
    public interface ICartRepository
    {
        // Get cart for user
        Task<Cart> GetCartAsync(string userId);

        // Add product to cart
        Task AddToCartAsync(string userId, int productId, int quantity, decimal price);

        // Update quantity for product in cart
        Task UpdateQuantityAsync(string userId, int cartItemId, int quantity);

        // Remove product from cart
        Task RemoveFromCartAsync(string userId, int cartItemId);

        // Clear entire cart
        Task ClearCartAsync(string userId);
    }
}
