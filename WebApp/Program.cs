using Core.Data;
using Core.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlite("Data Source=../store.db"));

builder.Services.AddMassTransit(x =>
{
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
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();