using Microsoft.AspNetCore.Mvc;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents.DashboardViewComponents;

public class _DashboardWidgetsComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _DashboardWidgetsComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task <IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var dto = new ReservationStatisticsDto
        {
            TotalCount = await GetReservationStatisticsAsync(client, "Reservations/GetTotalReservation"),
            PendingCount = await GetReservationStatisticsAsync(client, "Reservations/GetPendingReservation"),
            ApprovedCount = await GetReservationStatisticsAsync(client, "Reservations/GetApprovedReservation"),
            TotalCustomerCount = await GetReservationStatisticsAsync(client, "Reservations/GetTotalCustomerCount"),
        };

        return View(dto);
    }

    private static async Task<int> GetReservationStatisticsAsync(HttpClient client, string url)
    {
        using var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return int.Parse(content);
    }
}
