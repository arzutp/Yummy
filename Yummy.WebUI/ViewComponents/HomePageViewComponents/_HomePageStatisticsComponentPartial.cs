using Microsoft.AspNetCore.Mvc;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents.HomePageViewComponents;

public class _HomePageStatisticsComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _HomePageStatisticsComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var dto = new StatisticsDto
        {
            ProductCount = await GetStatitics(client, "Statistics/ProductCount"),
            ChefCount = await GetStatitics(client, "Statistics/ChefCount"),
            ReservationCount = await GetStatitics(client, "Statistics/ReservationCount"),
            TotalGuestCount = await GetStatitics(client, "Statistics/TotalGuestCount")
        };

        return View(dto);
    }

    private async Task<int> GetStatitics(HttpClient client, string url) 
    {
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return int.Parse(content);
    }
}
