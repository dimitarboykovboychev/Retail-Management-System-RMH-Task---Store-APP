using Core.Messages;
using MassTransit;

public class ProductCreatedConsumer: IConsumer<ProductCreated>
{
    public Task Consume(ConsumeContext<ProductCreated> context)
    {
        var message = context.Message;
        Console.WriteLine($"Product created: {message.Name} (${message.Price})");
        // Update UI, cache, or trigger workflow here
        return Task.CompletedTask;
    }
}