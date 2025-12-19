using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.ViewComponents;

public class _NavBarDefaultComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
