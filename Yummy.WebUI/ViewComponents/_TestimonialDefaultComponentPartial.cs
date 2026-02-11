using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.ViewComponents;

public class _TestimonialDefaultComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _TestimonialDefaultComponentPartial(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");
        var response = await client.GetAsync("Testimonials");
        if(!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var values = JsonConvert.DeserializeObject<List<ResultTestimonialDto>>(jsonData);
        return View(values);
    }
}
