using Core.Messages;
using Core.Services;
using MassTransit;
using MessageContracts;

namespace API.Consumers;

public class DeleteProductConsumer: IConsumer<DeleteProduct>
{
    private readonly IProductService _productService;
    private readonly ILogger<DeleteProductConsumer> _logger;
    private readonly MessageQueues _messageQueues;

    public DeleteProductConsumer(IProductService productService, ILogger<DeleteProductConsumer> logger, MessageQueues messageQueues)
    {
        _productService = productService;
        _logger = logger;
        _messageQueues = messageQueues;
    }

    public async Task Consume(ConsumeContext<DeleteProduct> context)
    {
        var message = context.Message;

        if (message.StoreID != _messageQueues.StoreID)
        {
            _logger.LogInformation("Ignoring DeleteProduct message for StoreId: {StoreId}", message.StoreID);

            return;
        }

        await _productService.DeleteProductAsync(message.ProductID);

        _logger.LogInformation("Deleted product with ProductId: {ProductId}", message.ProductID);

        await Task.CompletedTask;
    }
}