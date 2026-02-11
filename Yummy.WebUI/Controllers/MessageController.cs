using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Yummy.WebUI.Dtos;
using Yummy.WebUI.Models;

namespace Yummy.WebUI.Controllers;

public class MessageController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public MessageController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<IActionResult> MessageList(int page = 1)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var response = await client.GetAsync($"Messages/GetAllWithPagination?page={page}&pageSize=10");

        if (!response.IsSuccessStatusCode)
        {
            return View();
        }

        var jsonData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<PagedMessageResult>(jsonData);

        if (result == null)
        {
            return View(new PagedMessageResult
            {
                Items = new List<ResultMessageDto>(),
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
    public async Task<IActionResult> Create(CreateMessageDto createMessageDto)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createMessageDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await client.PostAsync("Messages/", stringContent);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction("MessageList");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        await client.DeleteAsync("Messages/" + id);

        return RedirectToAction("MessageList");
    }

    [HttpGet]
    public async Task<IActionResult> Detail(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync("Messages/" + id);

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdMessageDto>(jsonData);

        return View(value);
    }

    [HttpGet]
    public async Task<IActionResult> AnswerMessageWithOpenAi(int id)
    {
        var client = _httpClientFactory.CreateClient("YummyApi");

        var responseMessage = await client.GetAsync("Messages/" + id);

        var jsonData = await responseMessage.Content.ReadAsStringAsync();

        var value = JsonConvert.DeserializeObject<GetByIdMessageDto>(jsonData);

        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> AnswerMessageWithOpenAi(GetByIdMessageDto messageDto, string prompt)
    {
        prompt = messageDto.Detail;

        var apiKey = _configuration["OpenAI:ApiKey"];
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestData = new
        {
            model = "gpt-5.1",
            messages = new[]
            {
                new {
                    role = "system",
                    content = "Sen bir restoran için kullanıcıların göndermiş oldukları mesajlara detaylı " +
                    "ve olabildiğince olumlu, müşteri memnuniyeti gözeten cevaplar veren bir yapay zeka aracısın. " +
                    "Amacımız kullanıcı tarafından gönderilen mesajlara en olumlu ve mantıklı cevapları sunabilmek."
                },
                new {
                    role = "user",
                    content = prompt
                }
            },
            temperature = 0.5
        };

        var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);
        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
            var aiMessage = responseData.choices[0].message.content;
            ViewBag.Answer = aiMessage;
        }
        else
        {
            var errorText = await response.Content.ReadAsStringAsync();
            ViewBag.Answer = $"Error: {(int)response.StatusCode} {response.ReasonPhrase}\n{errorText}";
        }

        return View(messageDto);
    }

    public PartialViewResult SendMessage()
    {
        return PartialView();
    }


    [HttpPost] 
    public async Task<IActionResult> SendMessage(CreateMessageDto createMessageDto)
    {
        var client = new HttpClient();
        var apiKey = _configuration["OpenAI:SendMessageApiKey"];

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        try
        {
            var translateRequestBody = new
            {
                inputs = createMessageDto.Detail
            };
            var translateJson = System.Text.Json.JsonSerializer.Serialize(translateRequestBody);
            var translateContent = new StringContent(translateJson, Encoding.UTF8, "application/json");

            var translateResponse = await client.PostAsync("https://api-inference.huggingface.co/models/Helsinki-NLP/opus-mt-tr-en", translateContent);
            var translateResponseString = await translateResponse.Content.ReadAsStringAsync();

            string englishText = createMessageDto.Detail;
            if (translateResponseString.TrimStart().StartsWith("["))
            {
                var translateDoc = JsonDocument.Parse(translateResponseString);
                englishText = translateDoc.RootElement[0].GetProperty("translation_text").GetString();              
            }

            var toxicRequestBody = new
            {
                inputs = englishText
            };

            var toxicJson = System.Text.Json.JsonSerializer.Serialize(toxicRequestBody);
            var toxicContent = new StringContent(toxicJson, Encoding.UTF8, "application/json");
            var toxicResponse = await client.PostAsync("https://api-inference.huggingface.co/models/unitary/toxic-bert", toxicContent);
            var toxicResponseString = await toxicResponse.Content.ReadAsStringAsync();

            if (toxicResponseString.TrimStart().StartsWith("[")) 
            {
                var toxicDoc = JsonDocument.Parse(toxicResponseString);
                foreach (var item in toxicDoc.RootElement[0].EnumerateArray())
                {
                    string label = item.GetProperty("label").GetString();
                    double score = item.GetProperty("score").GetDouble();

                    if (score > 0.5) 
                    {
                        createMessageDto.Status = "Toksik Mesaj";
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(createMessageDto.Status)) 
            {
                createMessageDto.Status = "Mesaj Alındı";
            }
            
        }
        catch
        {
            createMessageDto.Status = "Onay Bekliyor";
        }

        var clientFactory = _httpClientFactory.CreateClient("YummyApi");

        var jsonData = JsonConvert.SerializeObject(createMessageDto);
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var responseMessage = await clientFactory.PostAsync("Messages/", stringContent);

        if (!responseMessage.IsSuccessStatusCode)
        {
            return View();
        }

        return RedirectToAction("Index", "Default");
    }
}