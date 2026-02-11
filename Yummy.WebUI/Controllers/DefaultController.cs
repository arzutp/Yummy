using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;
using Yummy.WebUI.Enums;

namespace Yummy.WebUI.Controllers;
public class DefaultController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DefaultController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CreateReservationDto createReservationDto)
    {
        createReservationDto.Status = ReservationStatus.Wait;

        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createReservationDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("Reservations", stringContent);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return View();     
        }

        return RedirectToAction("Index", "Default");
    }
}
