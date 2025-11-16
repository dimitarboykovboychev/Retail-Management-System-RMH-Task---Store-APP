using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Models;
using Core.Services;

namespace StoreWebApp.Pages;

public class IndexModel: PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
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

        bool isDeleted = await _productService.DeleteProduct(Guid.TryParse(ProductId, out var productId) ? productId : Guid.Empty);

        return isDeleted ? RedirectToPage("Index") : Page();
    }
}
