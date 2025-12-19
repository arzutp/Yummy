using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.ViewComponents.DefaultMenuViewComponents;

public class _DefaultMenuViewComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
