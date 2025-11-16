namespace Core.Messages;

public record ProductCreated(Guid ProductId, string Name, decimal Price);
