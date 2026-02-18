using Newtonsoft.Json;
using Yummy.WebUI.Dtos;

namespace Yummy.WebUI.Dtos;

public class DailyMenuDto
{
    [JsonProperty("starter")]
    public List<MenuItemDto> Starter { get; set; } = new();

    [JsonProperty("main")]
    public List<MenuItemDto> Main { get; set; } = new();

    [JsonProperty("dessert")]
    public List<MenuItemDto> Dessert { get; set; } = new();
}