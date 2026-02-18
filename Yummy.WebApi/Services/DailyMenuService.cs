using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Yummy.WebApi.Dtos.DailyMenusDtos;

namespace Yummy.WebApi.Services;

public class DailyMenuService : IDailyMenuService
{
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public DailyMenuService(IDistributedCache cache, IConfiguration configuration)
    {
        _cache = cache;
        _configuration = configuration;
    }

    public async Task<DailyMenuDto> GetDailyMenuAsync()
    {
        var key = $"daily-menu:{DateTime.UtcNow:yyyy-MM-dd}";

        string? cached = null;

        try
        {
            cached = await _cache.GetStringAsync(key);
        }
        catch
        {
            return GetFallbackMenu();
        }

        if (!string.IsNullOrEmpty(cached))
        {
            return JsonConvert.DeserializeObject<DailyMenuDto>(cached);
        }

        var newMenu = await GetMenuFromAI();

        var json = JsonConvert.SerializeObject(newMenu);

        var timeUntilMidnight = DateTime.UtcNow.Date
            .AddDays(1)
            .Subtract(DateTime.UtcNow);

        await _cache.SetStringAsync(
            key,
            json,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeUntilMidnight
            });

        return newMenu;
    }

    private async Task<DailyMenuDto> GetMenuFromAI()
    {
        try
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var month = DateTime.Now.Month;
            var season = month >= 3 && month <= 5 ? "İlkbahar" :
                         month >= 6 && month <= 8 ? "Yaz" :
                         month >= 9 && month <= 11 ? "Sonbahar" : "Kış";

            var prompt = $@"Sen deneyimli bir Türk mutfağı şefisin. Bugün {season} mevsimindeyiz.
                        Bir restoran için mevsimsel ve popüler yemeklerden oluşan günlük menü öner.

                        SADECE aşağıdaki JSON formatında cevap ver, başka hiçbir şey yazma:
                        {{
                          ""starter"": [
                            {{""name"": ""Yemek adı"", ""desc"": ""Kısa açıklama max 60 karakter"", ""emoji"": ""🍲"", ""time"": ""15 dk"", ""tags"": [""mevsimsel""]}},
                            {{""name"": ""Yemek adı"", ""desc"": ""Kısa açıklama max 60 karakter"", ""emoji"": ""🥗"", ""time"": ""10 dk"", ""tags"": [""popüler""]}}
                          ],
                         ""main"": [
                            {{""name"": ""Yemek adı"", ""desc"": ""Kısa açıklama max 60 karakter"", ""emoji"": ""🍖"", ""time"": ""35 dk"", ""tags"": [""mevsimsel"", ""popüler""]}},
                            {{""name"": ""Yemek adı"", ""desc"": ""Kısa açıklama max 60 karakter"", ""emoji"": ""🥘"", ""time"": ""40 dk"", ""tags"": [""popüler""]}}
                          ],
                         ""dessert"": [
                            {{""name"": ""Yemek adı"", ""desc"": ""Kısa açıklama max 60 karakter"", ""emoji"": ""🍮"", ""time"": ""20 dk"", ""tags"": [""mevsimsel""]}}
                          ]
                         }}";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                max_tokens = 1000,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            var responseData = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseData);
            var text = result.choices[0].message.content.ToString();

            var menu = JsonConvert.DeserializeObject<DailyMenuDto>(text);
            return menu;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AI Hatası: {ex.Message}");

            return GetFallbackMenu();
        }
    }

    private DailyMenuDto GetFallbackMenu()
    {
        return new DailyMenuDto
        {
            Starter = new List<MenuItemDto>
            {
                new MenuItemDto { Name = "Mercimek Çorbası", Desc = "Geleneksel Türk mutfağının vazgeçilmezi", Emoji = "🍲", Time = "15 dk", Tags = new List<string> { "popüler" } },
                new MenuItemDto { Name = "Mevsim Salatası", Desc = "Taze mevsim sebzeleriyle", Emoji = "🥗", Time = "10 dk", Tags = new List<string> { "mevsimsel" } }
            },
            Main = new List<MenuItemDto>
            {
                new MenuItemDto { Name = "Kuzu Güveç", Desc = "Yavaş pişirilmiş sebzeli kuzu eti", Emoji = "🍖", Time = "45 dk", Tags = new List<string> { "mevsimsel", "popüler" } },
                new MenuItemDto { Name = "Izgara Tavuk", Desc = "Baharatlı marine edilmiş tavuk göğsü", Emoji = "🍗", Time = "30 dk", Tags = new List<string> { "popüler" } }
            },
            Dessert = new List<MenuItemDto>
            {
                new MenuItemDto { Name = "Sütlaç", Desc = "Fırında pişirilmiş klasik sütlaç", Emoji = "🍮", Time = "20 dk", Tags = new List<string> { "popüler" } }
            }
        };
    }
}
