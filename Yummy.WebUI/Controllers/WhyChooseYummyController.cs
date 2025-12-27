using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;
public class WhyChooseYummyController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public WhyChooseYummyController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> WhyChooseYummyList()
    {
        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync("https://localhost:7114/api/Services");

        if (response == null)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ResultWhyChooseYummyDto>>(jsonData);

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateWhyChooseYummyDto createWhyChooseYummyDto)
    {
        var client = _httpClientFactory.CreateClient();

        var jsonData = JsonConvert.SerializeObject(createWhyChooseYummyDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("https://localhost:7114/api/Services", stringContent);

        if (responseMessage == null)
        {
            return View();
        }

        return RedirectToAction("WhyChooseYummyList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();

        await client.DeleteAsync($"https://localhost:7114/api/Services/{id}");

        return RedirectToAction("WhyChooseYummyList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync($"https://localhost:7114/api/Services/{id}");

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdWhyChooseYummyDto>(jsonData);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateWhyChooseYummyDto updateWhyChooseYummyDto)
    {
        var client = _httpClientFactory.CreateClient();

        var jsonData = JsonConvert.SerializeObject(updateWhyChooseYummyDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("https://localhost:7114/api/Services", stringContent);

        return RedirectToAction("WhyChooseYummyList");
    }
}
