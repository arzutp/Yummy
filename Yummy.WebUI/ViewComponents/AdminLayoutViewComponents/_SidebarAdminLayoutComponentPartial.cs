using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.ViewComponents.AdminLayoutViewComponents;

public class _SidebarAdminLayoutComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
