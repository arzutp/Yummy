using Microsoft.AspNetCore.Mvc;
using Yummy.WebApi.Context;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly ApiContext _context;

    public StatisticsController(ApiContext context)
    {
        _context = context;
    }

    [HttpGet("ProductCount")]
    public IActionResult ProductCount()
    {
        var count = _context.Products.Count();
        return Ok(count);
    }

    [HttpGet("ChefCount")]
    public IActionResult ChefCount() 
    { 
        return Ok(_context.Chefs.Count());
    }

    [HttpGet("ReservationCount")]
    public IActionResult ReservationCount() 
    { 
        return Ok(_context.Reservations.Count());
    }

    [HttpGet("TotalGuestCount")]
    public IActionResult TotalGuestCount()
    {
        return Ok(_context.Reservations.Sum(x => x.CountOfPeople));
    }
}
