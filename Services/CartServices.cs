using Microsoft.EntityFrameworkCore;
using SURE_Store_API.Data;
using SURE_Store_API.Models;


namespace SURE_Store_API.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        // عرض السلة الحالية للمستخدم
        public async Task<Cart> GetCart(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        // إضافة منتج للسلة
        public async Task<Cart> AddToCart(int userId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("الكمية يجب أن تكون أكبر من صفر.");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("المنتج غير موجود.");
            if (quantity > product.StockQuantity)
                throw new Exception("الكمية المطلوبة أكبر من المخزون.");

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (existingItem != null)
            {
                if (existingItem.Quantity + quantity > product.StockQuantity)
                    throw new Exception("لا يوجد مخزون كافٍ.");
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Cart = cart,
                });
            }

            await _context.SaveChangesAsync();
            return cart;
        }

        // تعديل كمية منتج موجود في السلة
        public async Task<Cart> UpdateQuantity(int userId, int cartItemId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("الكمية يجب أن تكون أكبر من صفر.");

            var cartItem = await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.Id == cartItemId);

            if (cartItem == null)
                throw new Exception("العنصر غير موجود في السلة.");
            if (quantity > cartItem.Product.StockQuantity)
                throw new Exception("الكمية المطلوبة أكبر من المخزون.");

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return await GetCart(userId);
        }

        // حذف منتج من السلة
        public async Task<Cart> RemoveFromCart(int userId, int cartItemId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Cart.UserId == userId && ci.Id == cartItemId);

            if (cartItem == null)
                throw new Exception("العنصر غير موجود في السلة.");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return await GetCart(userId);
        }
    }
}

