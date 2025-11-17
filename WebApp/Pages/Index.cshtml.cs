using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreWebApp.Pages;

public class IndexModel: PageModel
{
    private readonly IProductService _productService;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public IndexModel(IProductService productService, ISendEndpointProvider sendEndpointProvider)
    {
        _productService = productService;
        _sendEndpointProvider = sendEndpointProvider;
    }

    [BindProperty]
    public IList<Product> Products { get; set; } = new List<Product>();

    [BindProperty]
    public string ProductId { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _productService.GetProductsAsync();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        if (ProductId == null)
        {
            return Page();
        }

        bool isDeleted = await _productService.DeleteProductAsync(Guid.TryParse(ProductId, out var productId) ? productId : Guid.Empty);

        if(isDeleted)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{MessageQueues.ProductQueue}"));

            await endpoint.Send(new ProductDeleted(MessageQueues.StoreId, productId));

            return RedirectToPage("Index");
        }

        return Page();
    }
}
