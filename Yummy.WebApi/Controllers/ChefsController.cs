using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ChefsController : ControllerBase
{
    private readonly ApiContext _context;

    public ChefsController(ApiContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _context.Chefs.ToListAsync();
        return Ok(values);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Chef chef)
    {
        await _context.Chefs.AddAsync(chef);
        await _context.SaveChangesAsync();
        return Ok(chef);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Chefs.FirstOrDefaultAsync(c => c.Id == id);
        if (value == null)
        {
            return NotFound();
        }
        return Ok(value);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Chefs.FirstOrDefaultAsync(c => c.Id == id);
        if (value == null)
        {
            return NotFound();
        }
        _context.Chefs.Remove(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla silindi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(Chef chef)
    {
        var value = await _context.Chefs.FirstOrDefaultAsync(c => c.Id == chef.Id);
        if (value == null)
        {
            return NotFound();
        }
        value.NameSurname = chef.NameSurname;
        value.Title = chef.Title;
        value.Description = chef.Description;
        value.ImageUrl = chef.ImageUrl;
        await _context.SaveChangesAsync();

        return Ok("Güncelleme işlemi başarılı");
    }
}
