using Core.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Messages;

public record ProductCreated(Guid StoreId, Product Product);

public record CreateProduct(ProductExtended ProductExtended);

public static class MessageQueues
{
    public static readonly Guid StoreId;
    public static readonly string CreateProductQueue;
    public static readonly string RoutingKey;

    static MessageQueues()
    {
        StoreId = Guid.NewGuid();
        CreateProductQueue = $"store-{ StoreId.ToString() }";
        RoutingKey = $"{ StoreId.ToString() }.product";
    }

    public const string ProductCreatedQueue = "product-created-queue";
}
