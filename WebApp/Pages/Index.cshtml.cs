using Core.Messages;
using Core.Models;
using Core.Services;
using MassTransit;
using MessageContracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreWebApp.Pages;

public class IndexModel: PageModel
{
    private readonly IProductService _productService;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly MessageQueues _messageQueues;

    public IndexModel(IProductService productService, ISendEndpointProvider sendEndpointProvider, MessageQueues messageQueues)
    {
        _productService = productService;
        _sendEndpointProvider = sendEndpointProvider;
        _messageQueues = messageQueues;

        Products = new List<Product>();
    }

    [BindProperty]
    public IEnumerable<Product> Products { get; set; }

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
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{_messageQueues.CentralQueue}"));

            await endpoint.Send(new ProductDeleted(_messageQueues.StoreID, productId));

            return RedirectToPage("Index");
        }

        return Page();
    }
}
