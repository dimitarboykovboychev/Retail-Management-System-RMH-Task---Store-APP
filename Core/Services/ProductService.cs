using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class ProductService : IProductService
    {
        private readonly StoreDbContext _dbContext;

        public ProductService(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
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
                // logging can be added here

                throw;
            }   
        }

        public async Task<bool> CreateProduct(Product product)
        {
            try
            {
                if (product == null || !ValidateProduct(product))
                {
                    return false;
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
                        return false;
                    }

                    _dbContext.Products.Update(existingProduct);
                    await _dbContext.SaveChangesAsync();

                    return true;
                }

                product.ProductId = Guid.NewGuid();
                product.CreatedOn = DateTime.UtcNow;
                product.UpdatedOn = DateTime.UtcNow;

                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // logging can be added here

                throw;
            }
        }

        public async Task<bool> DeleteProduct(Guid productId)
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
            catch (Exception ex)
            {
                // logging can be added here

                throw;
            }
        }

        private bool ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name) || product.Price < 0 || product.MinPrice < 0)
            {
                // logging can be added here

                return false;
            }

            if (product.MinPrice > product.Price)
            {
                // logging can be added here

                return false;
            }

            if (product.Description != null && product.Description.Length > 500)
            {
                // logging can be added here

                return false;
            }

            if (product.Name.Length > 100)
            {
                // logging can be added here

                return false;
            }


            return true;
        }
    }
}
