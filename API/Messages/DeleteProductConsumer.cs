using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;

public class DeleteProductConsumer: IConsumer<DeleteProduct>
{
    private readonly IProductService _productService;
    private readonly ILogger<DeleteProductConsumer> _logger;

    public DeleteProductConsumer(IProductService productService, ILogger<DeleteProductConsumer> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DeleteProduct> context)
    {
        var message = context.Message;

        if (message.StoreId != MessageQueues.StoreId)
        {
            _logger.LogInformation("Ignoring DeleteProduct message for StoreId: {StoreId}", message.StoreId);

            return;
        }

        await _productService.DeleteProductAsync(message.ProductId);

        _logger.LogInformation("Deleted product with ProductId: {ProductId}", message.ProductId);

        await Task.CompletedTask;
    }
}