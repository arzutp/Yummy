using Newtonsoft.Json;

namespace Yummy.WebUI.Dtos;

public class MenuItemDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("desc")]
    public string Desc { get; set; }

    [JsonProperty("emoji")]
    public string Emoji { get; set; }

    [JsonProperty("time")]
    public string Time { get; set; }

    [JsonProperty("tags")]
    public List<string> Tags { get; set; } = new();
}
