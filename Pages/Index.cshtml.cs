using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Data;
using Core.Models;

namespace StoreWebApp.Pages;

public class IndexModel: PageModel
{
    public IndexModel()
    {
        
    }

    public IList<Product> Products { get; set; } = new List<Product>();

    public void OnGet()
    {
        
    }
}
