using SURE_Store_API.Models;
using System.Threading.Tasks;

namespace SURE_Store_API.Interfaces
{
    public interface ICartRepository
    {
        // جلب السلة للمستخدم
        Task<Cart> GetCart(int userId);

        // إضافة منتج للسلة
        Task<Cart> AddToCart(int userId, int productId, int quantity);

        // تحديث الكمية لمنتج في السلة
        Task<Cart> UpdateCartItem(int userId, int productId, int quantity);

        // حذف منتج من السلة
        Task<Cart> RemoveFromCart(int userId, int productId);

        // إفراغ السلة بالكامل
        Task<bool> ClearCart(int userId);
    }
}
