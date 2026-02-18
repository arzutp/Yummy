using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;
public class NotificationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NotificationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> NotificationList(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync($"Notifications/GetAllWithPagination?page={page}&pageSize=10");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedNotificationResult>(jsonData);

        if (result == null)
        {
            return View(new PagedNotificationResult
            {
                Items = new List<ResultNotificationDto>(),
                Page = 1,
                PageSize = 10,
                TotalPages = 0
            });
        }

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNotificationDto createNotificationDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createNotificationDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseNotification = await client.PostAsync("Notifications/", stringContent);

        if (!responseNotification.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction("NotificationList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        await client.DeleteAsync("Notifications/" + id);

        return RedirectToAction("NotificationList");
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseNotification = await client.GetAsync("Notifications/" + id);

        var jsonData = await responseNotification.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdNotificationDto>(jsonData);

        return View(value);
    }
}
