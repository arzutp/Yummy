using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;
public class GalleryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GalleryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> ImageList()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync("Images");

        if (response == null)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<List<ResultImageDto>>(jsonData);

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateImageDto createImageDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createImageDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("Images/", stringContent);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction("ImageList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        await client.DeleteAsync("Images/" + id);

        return RedirectToAction("ImageList");
    }
}
