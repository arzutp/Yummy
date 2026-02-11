using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Controllers;
public class ContactController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ContactController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> ContactList()
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync("Contacts");

        if (!response.IsSuccessStatusCode)
        {
            return View();
            
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var contactList = JsonConvert.DeserializeObject<List<ResultContactDto>>(jsonData);

        return View(contactList);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateContactDto createContactDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createContactDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("Contacts", stringContent);

        if (responseMessage == null)
        {
            return View();
        }

        return RedirectToAction("ContactList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        await client.DeleteAsync($"Contacts/{id}");

        return RedirectToAction("ContactList");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync($"Contacts/{id}");

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdContactDto>(jsonData);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateContactDto updateContactDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(updateContactDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responeMessage = await client.PutAsync("Contacts", stringContent);

        return RedirectToAction("ContactList");
    }
}
