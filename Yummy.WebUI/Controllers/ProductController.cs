using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;

public class ProductController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> ProductList(int page = 1)
    {
        var client = _httpClientFactory.CreateClient();

        var response = await client.GetAsync($"https://localhost:7114/api/Products/ProductWithCategory?page={page}&pageSize=10");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedProductResult>(jsonData);

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
    public async Task<IActionResult> Create()
    {
        var client = _httpClientFactory.CreateClient();

        var categories = await GetCategories(client);

        CreateProductDto createProductDto = new CreateProductDto
        {
            Categories = (from x in categories
                          select new SelectListItem
                          {
                              Text = x.Name,
                              Value = x.Id.ToString()
                          }).ToList()
        };

        return View(createProductDto);
        
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto createProductDto)
    {
        var client = _httpClientFactory.CreateClient();

        var jsonData = JsonConvert.SerializeObject(createProductDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("https://localhost:7114/api/Products", stringContent);
        if (!responseMessage.IsSuccessStatusCode)
        {

            return View();
        }
        return RedirectToAction("ProductList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient();

        await client.DeleteAsync($"https://localhost:7114/api/Products/{id}");

        return RedirectToAction("ProductList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient();

        var responseMessage = await client.GetAsync("https://localhost:7114/api/Products/" + id);
        var jsonData = await responseMessage.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<GetProductByIdDto>(jsonData);

        var categories = await GetCategories(client);

        UpdateProductDto updateProductDto = new UpdateProductDto
        {
            Categories = (from x in categories
                          select new SelectListItem
                          {
                              Text = x.Name,
                              Value = x.Id.ToString()
                          }).ToList(),
            Description = value.Description,
            CategoryId = value.CategoryId,
            Id = value.Id,
            ImageUrl = value.ImageUrl,
            Name = value.Name,
            Price = value.Price,
        };

        return View(updateProductDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto updateProductDto)
    {
        var client = _httpClientFactory.CreateClient();

        var jsonData = JsonConvert.SerializeObject(updateProductDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("https://localhost:7114/api/Products", stringContent);

        return RedirectToAction("ProductList");
    }

    #region private methods
    private async Task<List<ResultCategoryDto>> GetCategories(HttpClient client)
    {
        var responseMessage = await client.GetAsync("https://localhost:7114/api/Categories");

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new List<ResultCategoryDto>();
        }

        var jsonData = await responseMessage.Content.ReadAsStringAsync();
        var categories = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);

        return categories;
    }
    #endregion
}
