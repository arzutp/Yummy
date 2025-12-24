using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.ViewComponents.AdminLayoutViewComponents;

public class _RightSettingsAdminLayoutComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
