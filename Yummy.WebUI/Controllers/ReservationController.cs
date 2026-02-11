using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;

public class ReservationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ReservationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> ReservationList(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync($"Reservations/?page={page}&pageSize=10");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedReservastionResult>(jsonData);

        if (result == null)
        {
            return View(new PagedReservastionResult
            {
                Items = new List<ResultReservationDto>(),
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
    public async Task<IActionResult> Create(CreateReservationDto createReservationDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createReservationDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("Reservations/", stringContent);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction("ReservationList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");
        await client.DeleteAsync("Reservations/" + id);
        return RedirectToAction("ReservationList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync("Reservations/" + id);
        var jsonData = await responseMessage.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<GetByIdReservationDto>(jsonData);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateReservationDto updateReservationDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(updateReservationDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("Reservations", stringContent);

        return RedirectToAction("ReservationList");
    }
}
