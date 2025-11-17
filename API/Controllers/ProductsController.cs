using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
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
                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{MessageQueues.ProductQueue}"));

                await endpoint.Send(new ProductCreated(MessageQueues.StoreId, createdProduct));
            }

            return Ok(createdProduct);
        }

        [HttpDelete("{storeId}/{productId}")]
        public async Task<IActionResult> Delete(Guid storeId, Guid productId)
        {
            if (storeId != MessageQueues.StoreId)
            {
                return BadRequest("Invalid StoreId");
            }

            bool isDeleted = await _productService.DeleteProductAsync(productId);

            if (isDeleted)
            {
                var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{MessageQueues.ProductQueue}"));
                await endpoint.Send(new ProductDeleted(MessageQueues.StoreId, productId));
                
                return Ok();
            }
            
            return NotFound();
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

