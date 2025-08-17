using SURE_Store_API.Data;
using SURE_Store_API.DTOs.Product;
using SURE_Store_API.DTOs.Category;
using SURE_Store_API.DTOs;
using SURE_Store_API.Models;
using Microsoft.EntityFrameworkCore;

namespace SURE_Store_API.Services
{
    // Interface defining the contract for product service operations
    // This interface allows for dependency injection and unit testing
    public interface IProductService
    { //
        Task<ProductListResponse> GetProductsAsync(int page = 1, int pageSize = 10, int? categoryId = null, string? search = null);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto request);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductRequest request);
        Task<bool> DeleteProductAsync(int id);
        Task<List<CategoryDto>> GetCategoriesAsync();
    }


    public class ProductService : IProductService
    {
        // Dependency injection field for database context
        private readonly ApplicationDbContext _context;  // Database context for Entity Framework operations

        // Constructor for dependency injection of database context
        public ProductService(ApplicationDbContext context)  // Inject database context
        {
            _context = context;  // Store database context reference
        }


        // This method handles product catalog browsing with search and filtering capabilities
        public async Task<ProductListResponse> GetProductsAsync(int page = 1, int pageSize = 10, int? categoryId = null, string? search = null)
        {
            // Start building the query to get products with their categories included
            var query = _context.Products  // Get products from database
                .Include(p => p.Category)  // Include category information in the query (eager loading)
                .Where(p => p.StockQuantity > 0);  // Filter to show only products with stock available


            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);  // Filter products by category ID
            }


            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) ||
                                       p.Description.ToLower().Contains(searchTerm));
            }


            var totalCount = await query.CountAsync();  // Count total matching products
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);  // Calculate total number of pages

            // Get the actual products for the current page with pagination
            var products = await query
                .Skip((page - 1) * pageSize)  // Skip products from previous pages
                .Take(pageSize)  // Take only products for current page
                .Select(p => new ProductDto  // Transform database entities to DTOs
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    StockQuantity = p.StockQuantity,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name
                })
                .ToListAsync();  // Execute query and return list

            // Return paginated response with products and pagination metadata
            return new ProductListResponse
            {
                Products = products,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }


        // This method handles product detail viewing
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {

            var product = await _context.Products
                .Include(p => p.Category)  // Include category information (eager loading)
                .FirstOrDefaultAsync(p => p.Id == id);


            if (product == null)
                return null;

            // Transform database entity to DTO and return
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            };
        }


        public async Task<ProductDto> CreateProductAsync(CreateProductDto request)
        {
            // Create new product entity from request data
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId
            };


            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Load category information for the created product
            await _context.Entry(product)  // Get entity entry for the product
                .Reference(p => p.Category)  // Reference the category navigation property
                .LoadAsync();  // Load category data from database

            // Transform created entity to DTO and return
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            };
        }


        public async Task<ProductDto> UpdateProductAsync(int id, UpdateProductRequest request)
        {

            var product = await _context.Products  // Get products from database
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);


            if (product == null)
                throw new ArgumentException("Product not found");


            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;
            product.StockQuantity = request.StockQuantity;
            product.CategoryId = request.CategoryId;


            await _context.SaveChangesAsync();  // Persist changes to database

            // Transform updated entity to DTO and return
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name
            };
        }


        public async Task<bool> DeleteProductAsync(int id)
        {

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);


            if (product == null)
                return false;


            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            // Query all categories from database and transform to DTOs
            return await _context.Categories
                .Select(c => new CategoryDto  // Transform to DTOs
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();  // Execute query and return list
        }
    }
}
