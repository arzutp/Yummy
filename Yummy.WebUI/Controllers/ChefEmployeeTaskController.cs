using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;
public class ChefEmployeeTaskController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChefEmployeeTaskController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> ChefEmployeeTaskList(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync($"ChefEmployeeTasks/GetAllWithPagination?page={page}&pageSize=10");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedChefEmployeeTaskResult>(jsonData);

        if (result == null)
        {
            return View(new PagedChefEmployeeTaskResult
            {
                Items = new List<ResultChefEmployeeTaskDto>(),
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
        var client = _httpClientFactory.CreateClient("YummyApi");

        var chefs = await GetChefs(client);
        var tasks = await GetTasks(client);

        CreateChefEmployeeTaskDto createChefEmployeeTaskDto = new CreateChefEmployeeTaskDto
        {
            Chefs = (from x in chefs
                     select new SelectListItem
                     {
                         Text = x.NameSurname,
                         Value = x.Id.ToString()
                     }).ToList(),

            Tasks = (from x in tasks
                     select new SelectListItem
                     {
                         Text = x.Name,
                         Value = x.Id.ToString()
                     }).ToList(),
            DueDate = DateTime.Now
        };

        return View(createChefEmployeeTaskDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateChefEmployeeTaskDto createChefEmployeeTaskDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createChefEmployeeTaskDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("ChefEmployeeTasks/", stringContent);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction("ChefEmployeeTaskList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        await client.DeleteAsync("ChefEmployeeTasks/" + id);

        return RedirectToAction("ChefEmployeeTaskList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync("ChefEmployeeTasks/" + id);
        var jsonData = await responseMessage.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<GetByIdChefEmployeeTaskDto>(jsonData);

        var chefs = await GetChefs(client);
        var tasks = await GetTasks(client);

        GetByIdChefEmployeeTaskDto getByIdChefEmployeeTaskDto = new GetByIdChefEmployeeTaskDto
        {
            Chefs = (from x in chefs
                     select new SelectListItem
                     {
                         Text = x.NameSurname,
                         Value = x.Id.ToString()
                     }).ToList(),

            Tasks = (from x in tasks
                     select new SelectListItem
                     {
                         Text = x.Name,
                         Value = x.Id.ToString()
                     }).ToList(),
            DueDate = value.DueDate,
            AssignDate = value.AssignDate,
            ChefId = value.ChefId,
            EmployeeTaskId = value.EmployeeTaskId,
            TaskStatus = value.TaskStatus
        };

        return View(getByIdChefEmployeeTaskDto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateChefEmployeeTaskDto updateChefEmployeeTaskDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(updateChefEmployeeTaskDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("ChefEmployeeTasks", stringContent);

        return RedirectToAction("ChefEmployeeTaskList");
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");
        var response = await client.GetAsync($"ChefEmployeeTasks/GetByTaskId/{id}");

        var values = new LastTaskSummaryDto();

        if (!response.IsSuccessStatusCode)
        {
            return View(values);
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        values = JsonConvert.DeserializeObject<LastTaskSummaryDto>(jsonData);
        return View(values);
    }

    #region get chefs methods
    private async Task<List<ResultChefDto>> GetChefs(HttpClient client)
    {
        var responseMessage = await client.GetAsync("Chefs");

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new List<ResultChefDto>();
        }

        var jsonData = await responseMessage.Content.ReadAsStringAsync();
        var chefs = JsonConvert.DeserializeObject<List<ResultChefDto>>(jsonData);

        return chefs;
    }

    private async Task<List<ResultEmployeeTaskDto>> GetTasks(HttpClient client)
    {
        var responseMessage = await client.GetAsync("EmployeeTasks");

        if (!responseMessage.IsSuccessStatusCode)
        {
            return new List<ResultEmployeeTaskDto>();
        }

        var jsonData = await responseMessage.Content.ReadAsStringAsync();
        var tasks = JsonConvert.DeserializeObject<List<ResultEmployeeTaskDto>>(jsonData);

        return tasks;
    }
    #endregion
}
