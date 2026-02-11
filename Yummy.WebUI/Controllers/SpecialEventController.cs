using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;

public class SpecialEventController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SpecialEventController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> SpecialEventList()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync("SpecialEvents");

        if (response == null)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ResultSpecialEventDto>>(jsonData);

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSpecialEventDto createSpecialEventDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createSpecialEventDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("SpecialEvents", stringContent);

        if (responseMessage == null)
        {
            return View();
        }

        return RedirectToAction("SpecialEventList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        await client.DeleteAsync($"SpecialEvents/{id}");

        return RedirectToAction("SpecialEventList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync($"SpecialEvents/{id}");

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdSpecialEventDto>(jsonData);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateSpecialEventDto updateSpecialEventDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(updateSpecialEventDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("SpecialEvents", stringContent);

        return RedirectToAction("SpecialEventList");
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync("SpecialEvents/" + id);

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdSpecialEventDto>(jsonData);

        return View(value);
    }
}
