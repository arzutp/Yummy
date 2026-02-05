using Microsoft.AspNetCore.Mvc;

namespace Yummy.WebUI.Controllers;
public class ChatController : Controller
{
    public IActionResult SendChatWithAI()
    {
        return View();
    }
}
