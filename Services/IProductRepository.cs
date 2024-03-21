using ParanumusTask.Models;

namespace ParanumusTask.Services
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetAsync(int id);
        Task<Product> AddAsync(Product item);
        Task RemoveAsync(int id);
        Task<bool> UpdateAsync(Product item);
    }
}
