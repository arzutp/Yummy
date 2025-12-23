using Microsoft.AspNetCore.Mvc;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents.AdminLayoutViewComponents;

public class _NavbarMessageListAdminLayoutComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;
    public _NavbarMessageListAdminLayoutComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://localhost:7114/api/Messages/GetUnreadMesssages");
        if (!response.IsSuccessStatusCode)
        {
            return View();
        }
        var jsonData = await response.Content.ReadAsStringAsync();
        var values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);
        return View(values);
    }
}
