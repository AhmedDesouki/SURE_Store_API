using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SURE_Store_API.Data;
using SURE_Store_API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SURE_Store_API.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        // عرض السلة الحالية للمستخدم
        public async Task<Cart?> GetCart(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // إضافة منتج للسلة
        public async Task<Cart> AddToCart(string userId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("الكمية يجب أن تكون أكبر من صفر.");

            // حاولنا نجيب السلة أولًا (تحتوي CartItems + Product)
            var cart = await GetCart(userId);

            // لو مفيش سلة، نُنشئ واحدة جديدة
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                // لا نحتاج SaveChanges هنا فوراً، لكن نعمل save بعد إضافة العنصر لضمان وجود الـ Cart.Id إذا احتجناه
                await _context.SaveChangesAsync();
                // نعيد تحميل السلة كاملة مع العناصر (حالياً فارغة)
                cart = await GetCart(userId) ?? cart;
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("المنتج غير موجود.");
            if (quantity > product.StockQuantity)
                throw new Exception("الكمية المطلوبة أكبر من المخزون.");

            // نحاول نلاقي العنصر داخل السلة
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > product.StockQuantity)
                    throw new Exception("لا يوجد مخزون كافٍ.");
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    // لا حاجة لربط Cart يدوياً إذا كانت الـ Cart مُتعقبة (tracked) من _context
                };
                cart.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();

            // نعيد السلة المحدثة مع بيانات المنتجات
            return await GetCart(userId) ?? cart;
        }

        // تعديل كمية منتج موجود في السلة
        public async Task<Cart> UpdateQuantity(string userId, int cartItemId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("الكمية يجب أن تكون أكبر من صفر.");

            var cart = await GetCart(userId);
            if (cart == null)
                throw new Exception("السلة غير موجودة.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null)
                throw new Exception("العنصر غير موجود في السلة.");

            // product موجود لأن GetCart شمل ThenInclude(ci => ci.Product)
            if (quantity > (cartItem.Product?.StockQuantity ?? 0))
                throw new Exception("الكمية المطلوبة أكبر من المخزون.");

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return await GetCart(userId) ?? cart;
        }

        // حذف منتج من السلة
        public async Task<Cart> RemoveFromCart(string userId, int cartItemId)
        {
            var cart = await GetCart(userId);
            if (cart == null)
                throw new Exception("السلة غير موجودة.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null)
                throw new Exception("العنصر غير موجود في السلة.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return await GetCart(userId) ?? cart;
        }

        internal async Task<int?> GetCart(int v)
        {
            throw new NotImplementedException();
        }

        Task<Cart> ICartService.GetCart(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> AddToCart(int userId, int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> UpdateQuantity(int userId, int cartItemId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> RemoveFromCart(int userId, int cartItemId)
        {
            throw new NotImplementedException();
        }
    }
}
