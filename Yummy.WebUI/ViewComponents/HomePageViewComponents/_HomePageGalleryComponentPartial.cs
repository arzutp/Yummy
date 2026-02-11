using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents.HomePageViewComponents;

public class _HomePageGalleryComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _HomePageGalleryComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");
        var response = await client.GetAsync("Images");
        if (!response.IsSuccessStatusCode)
        {
            return View();
        }
        var jsonData = await response.Content.ReadAsStringAsync();
        var values = JsonConvert.DeserializeObject<List<ResultImageDto>>(jsonData);
        return View(values);
    }
}
