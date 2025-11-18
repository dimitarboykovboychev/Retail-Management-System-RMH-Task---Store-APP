using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;
using MessageContracts;

namespace API.Consumers;

public class CreateProductConsumer: IConsumer<CreateProduct>
{
    private readonly IProductService _productService;
    private readonly ILogger<CreateProductConsumer> _logger;
    private readonly MessageQueues _messageQueues;

    public CreateProductConsumer(IProductService productService, ILogger<CreateProductConsumer> logger, MessageQueues messageQueues)
    {
        _productService = productService;
        _logger = logger;
        _messageQueues = messageQueues;
    }

    public async Task Consume(ConsumeContext<CreateProduct> context)
    {
        var message = context.Message;

        if (message.Product.StoreID != _messageQueues.StoreID)
        {
            return;
        }

        var product = new Product
        {
            ProductId = message.Product.ProductID,
            Name = message.Product.Name,
            Description = message.Product.Description,
            Price = message.Product.Price,
            MinPrice = message.Product.MinPrice,
            CreatedOn = message.Product.CreatedOn,
            UpdatedOn = message.Product.UpdatedOn
        };

        await _productService.CreateProductAsync(product);

        _logger.LogInformation("Created product with ProductId: {ProductId}", product.ProductId);

        await Task.CompletedTask;
    }
}