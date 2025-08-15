using SURE_Store_API.DTOs.Product;
using SURE_Store_API.DTOs;



namespace SURE_Store_API.Interfaces
{
    public interface IProductService
    {
        Task<ProductListResponse> GetProductsAsync(int page = 1, int pageSize = 10, int? categoryId = null, string? search = null);
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<ProductDto> CreateProductAsync(CreateProductDto request);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductRequest request);
        Task<bool> DeleteProductAsync(int id);
        Task<List<CategoryDto>> GetCategoriesAsync();
    }
}
