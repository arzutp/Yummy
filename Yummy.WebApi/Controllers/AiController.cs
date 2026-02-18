using Microsoft.AspNetCore.Mvc;
using Yummy.WebApi.Services;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AiController : ControllerBase
{
    private readonly IDailyMenuService _dailyMenuService;

    public AiController(IDailyMenuService dailyMenuService)
    {
        _dailyMenuService = dailyMenuService;
    }

    [HttpGet("TodaysMenuWithAi")]
    public async Task<IActionResult> TodaysMenuWithAi()
    {
        var result = await _dailyMenuService.GetDailyMenuAsync();
        
        return Ok(result);
    }
}
