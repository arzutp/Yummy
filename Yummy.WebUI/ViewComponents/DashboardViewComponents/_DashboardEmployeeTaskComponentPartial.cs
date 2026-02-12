using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents.DashboardViewComponents;

public class _DashboardEmployeeTaskComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _DashboardEmployeeTaskComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");
        var response = await client.GetAsync("ChefEmployeeTasks/GetLast10Tasks");

        var values = new List<LastTaskSummaryDto> { new LastTaskSummaryDto() };

        if (!response.IsSuccessStatusCode)
        {
            return View(values);
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        values = JsonConvert.DeserializeObject<List<LastTaskSummaryDto>>(jsonData);
        return View(values);
    }
}
