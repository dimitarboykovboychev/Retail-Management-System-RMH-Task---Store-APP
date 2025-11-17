using Core.Messages;
using Core.Data;
using Core.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using API.Consumers;

var builder = WebApplication.CreateBuilder(args);

var storeID = builder.Configuration.GetValue<Guid>("Store:StoreID");
var centralQueue = builder.Configuration.GetValue<string>("Store:CentralQueue");

var messageQueues = new MessageQueues(storeID, centralQueue);

builder.Services.AddSingleton(messageQueues);

builder.Services.AddControllers();

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlite("Data Source=../store.db"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateProductConsumer>();
    x.AddConsumer<DeleteProductConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost";
        var username = builder.Configuration.GetValue<string>("RabbitMq:Username");
        var password = builder.Configuration.GetValue<string>("RabbitMq:Password");

        cfg.Host(host, h =>
        {
            if (!string.IsNullOrWhiteSpace(username)) h.Username(username);
            if (!string.IsNullOrWhiteSpace(password)) h.Password(password);
        });

        cfg.ReceiveEndpoint(messageQueues.StoreQueue, e =>
        {
            e.Bind("store-exchange", x =>
            {
                x.ExchangeType = "topic";
                x.RoutingKey = messageQueues.RoutingKey;
            });

            e.ConfigureConsumer<CreateProductConsumer>(context);
            e.ConfigureConsumer<DeleteProductConsumer>(context);
        });
    });
});

var app = builder.Build();

app.MapControllers();

app.Run();