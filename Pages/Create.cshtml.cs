using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Models;
using Core.Services;

namespace StoreWebApp.Pages;

public class CreateModel: PageModel
{
    private readonly IProductService _productService;

    public CreateModel(IProductService productService)
    {
        _productService = productService;
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

        bool isCreated = await _productService.CreateProduct(new Product()
        {
            Name = Name,
            Description = Description,
            Price = decimal.TryParse(Price, out var price) ? price : 0,
            MinPrice = decimal.TryParse(MinPrice, out var minPrice) ? minPrice : 0
        });

        return isCreated ? RedirectToPage("Index") : Page();
    }
}
