using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync("https://localhost:7114/api/Images");

        if (response == null)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultImageDto>>(jsonData);

        return View(result);
    }
}
