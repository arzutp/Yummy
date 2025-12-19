using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.ViewComponents;

public class _AboutDefaultComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
