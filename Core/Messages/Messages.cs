using Core.Models;

namespace Core.Messages;

public record ProductCreated(Guid StoreId, Product Product);
public record ProductDeleted(Guid StoreId, Guid ProductId);


public record CreateProduct(ProductExtended ProductExtended);

public record DeleteProduct(Guid StoreId, Guid ProductId);

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

    public const string ProductQueue = "product-queue";
}
