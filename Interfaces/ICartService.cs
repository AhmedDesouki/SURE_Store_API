using SURE_Store_API.DTOs;
using SURE_Store_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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

