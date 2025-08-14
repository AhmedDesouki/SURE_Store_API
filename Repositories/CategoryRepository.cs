using Microsoft.EntityFrameworkCore;
using SURE_Store_API.Data;
using SURE_Store_API.Interfaces;
using SURE_Store_API.Models;

namespace SURE_Store_API.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> GetProductCountAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .CountAsync();
        }
    }
}
