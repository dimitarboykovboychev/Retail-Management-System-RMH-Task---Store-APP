using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class ProductService: IProductService
    {
        private readonly StoreDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(StoreDbContext dbContext, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var products = new List<Product>();

            try
            {
                products = await _dbContext.Products.ToListAsync();

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving products.");

                throw;
            }
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                if (product == null || !ValidateProduct(product))
                {
                    return null;
                }

                if (await _dbContext.Products.AnyAsync(p => p.Name == product.Name))
                {
                    var existingProduct = await _dbContext.Products.SingleAsync(p => p.Name == product.Name);

                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.MinPrice = product.MinPrice;
                    existingProduct.UpdatedOn = DateTime.UtcNow;

                    if (!ValidateProduct(existingProduct))
                    {
                        return null;
                    }

                    _dbContext.Products.Update(existingProduct);
                    await _dbContext.SaveChangesAsync();

                    return existingProduct;
                }

                product.ProductId = Guid.NewGuid();
                product.CreatedOn = DateTime.UtcNow;
                product.UpdatedOn = DateTime.UtcNow;

                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");

                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            try
            {
                if (productId == Guid.Empty)
                {
                    return false;
                }

                var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.ProductId == productId);

                if (product != null)
                {
                    _dbContext.Products.Remove(product);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a product.");

                throw;
            }
        }

        private bool ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || product.Price < 0 || product.MinPrice < 0 || product.Price == 0 || product.MinPrice == 0)
            {
                _logger.LogWarning("Product {ProductId} has invalid Name, Price or MinPrice.", product.ProductId);

                return false;
            }

            if (product.MinPrice > product.Price)
            {
                _logger.LogWarning("Product {ProductId} has a MinPrice greater than its Price.", product.ProductId);

                return false;
            }

            if (product.Description != null && product.Description.Length > 500)
            {
                _logger.LogWarning("Product {ProductId} has a Description longer than 500 characters.", product.ProductId);

                return false;
            }

            if (product.Name.Length > 100)
            {
                _logger.LogWarning("Product {ProductId} has a Name longer than 100 characters.", product.ProductId);

                return false;
            }

            return true;
        }
    }
}
