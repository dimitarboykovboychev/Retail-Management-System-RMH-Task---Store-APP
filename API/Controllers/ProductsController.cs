using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace StoreAppWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController: ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, 
                                  IPublishEndpoint publishEndpoint, 
                                  IProductService productService)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            // Save to DB here...

            await _productService.CreateProduct(product);

            // Publish event
            //await _publishEndpoint.Publish(new ProductCreated(product.ProductId, product.Name, product.Price));

            return Ok(product);
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productService.GetProductsAsync();
        }
    }
}   

