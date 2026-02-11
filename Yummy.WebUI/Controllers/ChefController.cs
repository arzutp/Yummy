using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;

public class ChefController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChefController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> ChefList()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync("Chefs");

        if (response == null)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ResultChefDto>>(jsonData);

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateChefDto createChefDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createChefDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("Chefs", stringContent);

        if (responseMessage == null)
        {
            return View();
        }

        return RedirectToAction("ChefList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        await client.DeleteAsync($"Chefs/{id}");

        return RedirectToAction("ChefList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync($"Chefs/{id}");

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdChefDto>(jsonData);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateChefDto updateChefDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(updateChefDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("Chefs", stringContent);

        return RedirectToAction("ChefList");
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync("Chefs/" + id);

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdChefDto>(jsonData);

        return View(value);
    }
}
