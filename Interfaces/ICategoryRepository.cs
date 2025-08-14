namespace SURE_Store_API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> GetProductCountAsync(int categoryId);
    }
}
