using Core.Models;

namespace Core.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();

        Task<bool> CreateProduct(Product product);

        Task<bool> DeleteProduct(Guid productId);
    }
}
