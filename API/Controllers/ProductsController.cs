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
        private readonly IProductService _productService;
        private readonly ISendEndpointProvider _sendEndpointProvider;


        public ProductsController(IProductService productService, ISendEndpointProvider sendEndpointProvider)
        {
            _productService = productService;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var createdProduct = await _productService.CreateProductAsync(product);

            if(createdProduct != null)
            {
                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{MessageQueues.ProductCreatedQueue}"));

                await endpoint.Send(new ProductCreated(MessageQueues.StoreID, createdProduct));
            }

            return Ok(createdProduct);
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productService.GetProductsAsync();
        }

        [HttpGet]
        public IActionResult GetHealth() => Ok("StoreApp is healthy");
    }
}   

