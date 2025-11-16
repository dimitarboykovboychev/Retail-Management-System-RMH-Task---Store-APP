using Core.Models;

namespace Core.Messages;

public record ProductCreated(Guid StoreId, Product Product);

public static class MessageQueues
{
    public static readonly Guid StoreID;

    static MessageQueues()
    {
        StoreID = Guid.NewGuid();
    }

    public const string ProductCreatedQueue = "product-created-queue";
}
