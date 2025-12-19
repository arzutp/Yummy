using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.ViewComponents;

public class _FeatureDefaultComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
