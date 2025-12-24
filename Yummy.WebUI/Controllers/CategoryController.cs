using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;
public class CategoryController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CategoryController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> CategoryList(int page = 1)
    {
        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync($"https://localhost:7114/api/Categories?page={page}&pageSize=10");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedCategoryResult>(jsonData);

        if (result == null)
        {
            return View(new PagedCategoryResult
            {
                Items = new List<ResultCategoryDto>(),
                Page = 1,
                PageSize = 10,
                TotalPages = 0
            });
        }

        return View(result);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
    {
        var client = _httpClientFactory.CreateClient();

        var jsonData = JsonConvert.SerializeObject(createCategoryDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("https://localhost:7114/api/Categories", stringContent);
        
        if (!responseMessage.IsSuccessStatusCode)
        {
            return View();
        }
        
        return RedirectToAction("CategoryList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();
        await client.DeleteAsync("https://localhost:7114/api/Categories/" + id);
        return RedirectToAction("CategoryList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id) 
    {
        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync("https://localhost:7114/api/Categories/" + id);
        var jsonData = await responseMessage.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<GetCategoryByIdDto>(jsonData);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryDto updateCategoryDto)
    {
        var client = _httpClientFactory.CreateClient();

        var jsonData = JsonConvert.SerializeObject(updateCategoryDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("https://localhost:7114/api/Categories", stringContent);

        return RedirectToAction("CategoryList");
    }
}
