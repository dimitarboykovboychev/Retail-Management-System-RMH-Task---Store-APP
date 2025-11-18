using Core.Models;

namespace Core.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> CreateProductAsync(Product product);

        Task<bool> DeleteProductAsync(Guid productId);
    }
}
