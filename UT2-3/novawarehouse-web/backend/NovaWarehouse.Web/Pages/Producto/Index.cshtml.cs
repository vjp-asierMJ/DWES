using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaWarehouse.Web.Models;
namespace NovaWarehouse.Web.Pages;

public class ProductoModel : PageModel
{
    private const decimal VatRate = 0.21m;
    public Product Product { get; } = new Product()
    {
        Name = "Caja M",
        Price = 2.50m,
        Stock = 8

    };
    public decimal PriceWithVat { get; private set; }
    public bool LowStock { get; private set; }
    public void OnGet()
    {
        PriceWithVat = Product.Price * (1 + VatRate);
        LowStock = Product.Stock < 10;
    }
}