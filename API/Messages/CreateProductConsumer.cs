using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;

public class CreateProductConsumer: IConsumer<CreateProduct>
{
    private readonly IProductService _productService;

    public CreateProductConsumer(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<CreateProduct> context)
    {
        var message = context.Message;

        if (message.ProductExtended.StoreId != MessageQueues.StoreId)
        {
            return;
        }

        var product = new Product
        {
            ProductId = message.ProductExtended.ProductId,
            Name = message.ProductExtended.Name,
            Description = message.ProductExtended.Description,
            Price = message.ProductExtended.Price,
            MinPrice = message.ProductExtended.MinPrice,
            CreatedOn = message.ProductExtended.CreatedOn,
            UpdatedOn = message.ProductExtended.UpdatedOn
        };

        await _productService.CreateProductAsync(product);

        await Task.CompletedTask;
    }
}