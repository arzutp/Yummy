using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;

public class EmployeeTaskController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public EmployeeTaskController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> EmployeeTaskList(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync($"EmployeeTasks/GetAllWithPagination?page={page}&pageSize=10");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedEmployeeTaskResult>(jsonData);

        if (result == null)
        {
            return View(new PagedEmployeeTaskResult
            {
                Items = new List<ResultEmployeeTaskDto>(),
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
    public async Task<IActionResult> Create(CreateEmployeeTaskDto createEmployeeTaskDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createEmployeeTaskDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("EmployeeTasks/", stringContent);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction("EmployeeTaskList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");
        await client.DeleteAsync("EmployeeTasks/" + id);
        return RedirectToAction("EmployeeTaskList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync("EmployeeTasks/" + id);
        var jsonData = await responseMessage.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<GetByIdEmployeeTaskDto>(jsonData);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateEmployeeTaskDto updateEmployeeTaskDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(updateEmployeeTaskDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("EmployeeTasks", stringContent);

        return RedirectToAction("EmployeeTaskList");
    }
}
