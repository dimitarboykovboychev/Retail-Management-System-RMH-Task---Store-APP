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
    x.AddConsumer<CreateProductConsumer>();

    //x.UsingRabbitMq((context, cfg) =>
    //{
    //    var host = builder.Configuration.GetValue<string>("RabbitMq:Host") ?? "localhost";
    //    var username = builder.Configuration.GetValue<string>("RabbitMq:Username");
    //    var password = builder.Configuration.GetValue<string>("RabbitMq:Password");

    //    cfg.Host(host, h =>
    //    {
    //        if(!string.IsNullOrWhiteSpace(username)) h.Username(username);
    //        if(!string.IsNullOrWhiteSpace(password)) h.Password(password);
    //    });

    //    cfg.ReceiveEndpoint(MessageQueues.createProductQueue, e =>
    //    {
    //        e.Bind("store-exchange", x =>
    //        {
    //            x.ExchangeType = "topic";
    //            x.RoutingKey = MessageQueues.routingKey;
    //        });

    //        e.ConfigureConsumer<ProductCreatedConsumer>(context);
    //    });
    //});

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapGet("/", () => "App is running with in-memory transport!");
//app.MapControllers();

app.Run();