using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;

public class ProductCreatedConsumer: IConsumer<ProductCreated>
{
    private readonly IProductService _productService;

    public ProductCreatedConsumer(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        var message = context.Message;

        var product = new Product
        {
            ProductId = message.Product.ProductId,
            Name = message.Product.Name,
            Description = message.Product.Description,
            Price = message.Product.Price,
            MinPrice = message.Product.MinPrice,
            CreatedOn = message.Product.CreatedOn,
            UpdatedOn = message.Product.UpdatedOn
        };

        await _productService.CreateProductAsync(product);

        await Task.CompletedTask;
    }
}