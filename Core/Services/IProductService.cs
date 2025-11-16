using Core.Models;

namespace Core.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();

        Task<Product> CreateProductAsync(Product product);

        Task<bool> DeleteProductAsync(Guid productId);
    }
}
