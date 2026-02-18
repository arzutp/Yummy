using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents.DashboardViewComponents;

public class _DashboardGroupReservationComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _DashboardGroupReservationComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");
        var response = await client.GetAsync("GroupReservations/GetTodayReservation");

        var values = new List<ResultGroupReservationDto>();

        if (!response.IsSuccessStatusCode)
        {
            return View(values);
        }

        var responseData = await response.Content.ReadAsStringAsync();
        values = JsonConvert.DeserializeObject<List<ResultGroupReservationDto>>(responseData);

        return View(values);
    }
}
