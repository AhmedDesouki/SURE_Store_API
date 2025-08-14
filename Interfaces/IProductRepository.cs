using SURE_Store_API.Models;

namespace SURE_Store_API.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        //products by category 
        //return all the products for the category id=1 category name = electronics 
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);

        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
}
