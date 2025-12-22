using Microsoft.AspNetCore.Mvc;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents;

public class _ChefDefaultComponentPartial : ViewComponent
{ 
    private readonly IHttpClientFactory _httpClientFactory;
    public _ChefDefaultComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://localhost:7114/api/Chefs");
        if (!response.IsSuccessStatusCode)
        {
            return View();
        }
        var jsonData = await response.Content.ReadAsStringAsync();
        var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultChefDto>>(jsonData);
        return View(values);
    }
}
