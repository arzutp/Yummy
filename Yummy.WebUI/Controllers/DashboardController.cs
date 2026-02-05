using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.Controllers;
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
