using SURE_Store_API.DTOs.Product;
using SURE_Store_API.DTOs;



namespace SURE_Store_API.Interfaces
{
    public interface IProductService
    {
        Task<ProductListResponse> GetProductsAsync(int page = 1, int pageSize = 10, int? categoryId = null, string? search = null);  // Get paginated list of products with optional filtering
        Task<ProductDto?> GetProductByIdAsync(int id);  // Get single product by its unique identifier
        Task<ProductDto> CreateProductAsync(CreateProductDto request);  // Create new product in the system
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductRequest request);  // Update existing product information
        Task<bool> DeleteProductAsync(int id);  // Delete product from the system
        Task<List<CategoryDto>> GetCategoriesAsync();  // Get all available product categories
    }
}
