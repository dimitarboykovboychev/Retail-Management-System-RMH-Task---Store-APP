using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreWebApp.Pages;

public class CreateModel: PageModel
{
    private readonly IProductService _productService;
    private readonly ISendEndpointProvider _sendEndpointProvider;


    public CreateModel(IProductService productService, ISendEndpointProvider sendEndpointProvider)
    {
        _productService = productService;
        _sendEndpointProvider = sendEndpointProvider;
    }

    [BindProperty]
    public string Name { get; set; }

    [BindProperty]
    public string Price { get; set; }

    [BindProperty]
    public string MinPrice { get; set; }

    [BindProperty]
    public string Description { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var product = await _productService.CreateProductAsync(new Product()
        {
            Name = Name,
            Description = Description,
            Price = decimal.TryParse(Price, out var price) ? price : 0,
            MinPrice = decimal.TryParse(MinPrice, out var minPrice) ? minPrice : 0
        });

        if (product != null)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{MessageQueues.ProductCreatedQueue}"));

            await endpoint.Send(new ProductCreated(MessageQueues.StoreId, product));

            return RedirectToPage("Index");
        }

        return Page();
    }
}
