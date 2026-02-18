using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Yummy.WebUI.Models;

namespace Yummy.WebUI.Controllers;

public class AiController : Controller
{
    private readonly IConfiguration _configuration;

    public AiController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult CreateRecipeWithOpenAi()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecipeWithOpenAi(string prompt)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];         
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestData = new
        {
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { 
                    role = "system", 
                    content = "Sen bir restoran için yemek önerileri yapan bir yapay zeka asistanısın." +
                    "Amacımız kullanıcı tarafından girilen malzemelere göre yemek tarifi önerisinde bulunmak."
                },
                new {
                    role = "user",
                    content = prompt
                    }
            },
            temperature = 0.5
        };

        var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);

        if(response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
            var aiMessage = responseData.choices[0].message.content;
            ViewBag.Recipe = aiMessage;
        }
        else
        {
            var errorText = await response.Content.ReadAsStringAsync();
            ViewBag.Recipe = $"Error: {(int)response.StatusCode} {response.ReasonPhrase}\n{errorText}";

        }

        return View();
    }
}
