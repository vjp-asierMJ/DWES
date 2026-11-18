using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaWarehouse.Web.Models;
public class EstadoModel : PageModel
{
    public DateTime ServerTime { get; private set; }
    public int Visits { get; private set; }
    public void OnGet()
    {
        ServerTime = DateTime.Now;
        SiteStats.Visits++;
        Visits = SiteStats.Visits;
    }
}
