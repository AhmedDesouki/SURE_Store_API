using Microsoft.EntityFrameworkCore;
using SURE_Store_API.Data;
using SURE_Store_API.Interfaces;
using SURE_Store_API.Models;

namespace SURE_Store_API.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId)
                ?? new Cart { UserId = userId, CartItems = new List<CartItem>() };
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity, decimal price)
        {
            var cart = await GetCartAsync(userId);

            var existingItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem == null)
            {
                var newItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = price
                };
                cart.CartItems.Add(newItem);

                if (cart.Id == 0)
                {
                    _context.Carts.Add(cart);
                }
            }
            else
            {
                existingItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.CartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item != null)
            {
                item.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCartAsync(string userId, int cartItemId)
        {
            var cart = await GetCartAsync(userId);
            var item = cart.CartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (item != null)
            {
                cart.CartItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                await _context.SaveChangesAsync();
            }
        }

       
    }
}