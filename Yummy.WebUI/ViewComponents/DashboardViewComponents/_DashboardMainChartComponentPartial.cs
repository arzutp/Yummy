using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents.DashboardViewComponents;

public class _DashboardMainChartComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _DashboardMainChartComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var statsRes = await client.GetAsync("Reservations/GetReservationStats");
        statsRes.EnsureSuccessStatusCode();
        var statsJson = await statsRes.Content.ReadAsStringAsync();
        var chart = JsonConvert.DeserializeObject<List<ReservationChartDto>>(statsJson) ?? new();

        var sumRes = await client.GetAsync("Reservations/GetReservationSummaryLast4Months");
        sumRes.EnsureSuccessStatusCode();
        var sumJson = await sumRes.Content.ReadAsStringAsync();
        var summary = JsonConvert.DeserializeObject<ReservationSummaryDto>(sumJson) ?? new();

        return View(Tuple.Create(chart, summary));
    }

}
