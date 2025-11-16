using Core.Models;

namespace Core.Messages;

public record ProductCreated(Guid StoreId, Product Product);

public record CreateProduct(ProductExtended ProductExtended);

public static class MessageQueues
{
    public static readonly Guid StoreId;

    static MessageQueues()
    {
        StoreId = Guid.NewGuid();
    }

    public const string ProductCreatedQueue = "product-created-queue";
}
