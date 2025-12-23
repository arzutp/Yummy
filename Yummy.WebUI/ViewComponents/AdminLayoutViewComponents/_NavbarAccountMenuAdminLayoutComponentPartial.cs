using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.ViewComponents.AdminLayoutViewComponents;

public class _NavbarAccountMenuAdminLayoutComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
