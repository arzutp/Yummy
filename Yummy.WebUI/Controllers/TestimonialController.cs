using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;

public class TestimonialController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TestimonialController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> TestimonialList(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync($"Testimonials/GetAllWithPagination?page={page}&pageSize=15");

        if (!response.IsSuccessStatusCode) 
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedTestimonialResult>(jsonData);

        if (result == null)
        {
            return View(new PagedTestimonialResult
            {
                Items = new List<ResultTestimonialDto>(),
                Page = 1,
                PageSize = 10,
                TotalPages = 0
            });
        }

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync("Testimonials/" + id);

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdTestimonialDto>(jsonData);

        return View(value);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        await client.DeleteAsync("Testimonials/" + id);

        return RedirectToAction("TestimonialList");
    }

    //[TO-DO] : Anasayfada Create ve Update işlemleri olsa daha iyi olur diye düşünüyorum.
    //Bu yüzden admin tarafındakileri yorum satırına aldım.

    //[HttpGet]
    //public IActionResult Create()
    //{
    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> Create(CreateTestimonialDto createTestimonialDto)
    //{
    //    var client = _httpClientFactory.CreateClient("YummyApi");

    //    var jsonData = JsonConvert.SerializeObject(createTestimonialDto);
    //    StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

    //    var responseMessage = await client.PostAsync("Testimonials/", stringContent);

    //    if (!responseMessage.IsSuccessStatusCode)
    //    {
    //        return View();
    //    }

    //    return RedirectToAction("TestimonialList");
    //}

    //[HttpGet]
    //public async Task<IActionResult> Update(int id)
    //{
    //    var client = _httpClientFactory.CreateClient("YummyApi");

    //    var responseMessage = await client.GetAsync("Testimonials/" + id);

    //    var jsonData = await responseMessage.Content.ReadAsStringAsync();

    //    var value = JsonConvert.DeserializeObject<GetByIdTestimonialDto>(jsonData);

    //    return View(value);
    //}

    //[HttpPost]
    //public async Task<IActionResult> Update(UpdateTestimonialDto updateTestimonialDto)
    //{
    //    var client = _httpClientFactory.CreateClient("YummyApi");

    //    var jsonData = JsonConvert.SerializeObject(updateTestimonialDto);

    //    StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

    //    var responeMessage = await client.PutAsync("Testimonials", stringContent);

    //    return RedirectToAction("TestimonialList");
    //}
}
