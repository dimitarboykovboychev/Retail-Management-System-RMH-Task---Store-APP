using Core.Data;
using Core.Messages;
using Core.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlite("Data Source=../store.db"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProductCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost";
        var username = builder.Configuration.GetValue<string>("RabbitMq:Username");
        var password = builder.Configuration.GetValue<string>("RabbitMq:Password");

        cfg.Host(host, h =>
        {
            if(!string.IsNullOrWhiteSpace(username)) h.Username(username);
            if(!string.IsNullOrWhiteSpace(password)) h.Password(password);
        });

        // move this to MessagesQueues in Messages.cs
        var storeId = MessageQueues.StoreId.ToString();
        var queueName = $"store-{storeId}";

        cfg.ReceiveEndpoint(queueName, e =>
        {
            e.Bind("store-exchange", x =>
            {
                x.ExchangeType = "topic";
                x.RoutingKey = $"{storeId}.product";
            });

            e.ConfigureConsumer<ProductCreatedConsumer>(context);
        });
    });
});

var app = builder.Build();
app.MapControllers();
app.Run();