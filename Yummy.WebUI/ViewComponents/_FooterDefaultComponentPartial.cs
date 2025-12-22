using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.ViewComponents;

public class _FooterDefaultComponentPartial : ViewComponent
{ 
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
