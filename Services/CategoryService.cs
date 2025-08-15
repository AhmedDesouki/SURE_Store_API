using SURE_Store_API.Data;
using SURE_Store_API.DTOs.Category;
using SURE_Store_API.DTOs;
using SURE_Store_API.Models;
using Microsoft.EntityFrameworkCore;

namespace SURE_Store_API.Services
{
    // Interface defining the contract for category service operations
    // This interface allows for dependency injection and unit testing
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto request);
        Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(int id);
    }

    // Implementation of category service that handles category management
    // This class contains all business logic for category operations
    public class CategoryService : ICategoryService
    {
        // Dependency injection field for database context
        private readonly ApplicationDbContext _context;  // Database context for Entity Framework operations

        // Constructor for dependency injection of database context
        public CategoryService(ApplicationDbContext context)  // Inject database context
        {
            _context = context;  // Store database context reference
        }


        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            // Query all categories from database and transform to DTOs
            return await _context.Categories  // Get categories from database
                .Select(c => new CategoryDto  // Transform to DTOs
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();  // Execute query and return list
        }


        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            // Query the database to find category by ID
            var category = await _context.Categories  // Get categories from database
                .FirstOrDefaultAsync(c => c.Id == id);  // Find category with matching ID


            if (category == null)
                return null;

            // Transform database entity to DTO and return
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }


        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto request)
        {
            // Create new category entity from request data
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            // Add category to database context
            _context.Categories.Add(category);  // Mark category for insertion
            await _context.SaveChangesAsync();  // Save changes to database


            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }


        public async Task<CategoryDto> UpdateCategoryAsync(int id, UpdateCategoryRequest request)
        {
            // Find existing category by ID
            var category = await _context.Categories  // Get categories from database
                .FirstOrDefaultAsync(c => c.Id == id);  // Find category with matching ID


            if (category == null)
                throw new ArgumentException("Category not found");

            // Update category properties with new values from request
            category.Name = request.Name;
            category.Description = request.Description;


            await _context.SaveChangesAsync();


            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }


        public async Task<bool> DeleteCategoryAsync(int id)
        {
            // Find category by ID
            var category = await _context.Categories  // Get categories from database
                .FirstOrDefaultAsync(c => c.Id == id);  // Find category with matching ID


            if (category == null)
                return false;


            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

