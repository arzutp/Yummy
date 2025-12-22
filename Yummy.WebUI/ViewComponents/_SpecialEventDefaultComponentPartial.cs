using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents;

public class _SpecialEventDefaultComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _SpecialEventDefaultComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://localhost:7114/api/SpecialEvents");
        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var values = JsonConvert.DeserializeObject<List<ResultSpecialEventDto>>(jsonData);
        return View(values);
    }
}
