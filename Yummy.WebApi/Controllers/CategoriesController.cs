using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Yummy.WebApi.Context;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ApiContext _context;

    public CategoriesController(ApiContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _context.Categories.ToListAsync();
        return Ok(values);
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (value == null)
        {
            return NotFound();
        }
        return Ok(value);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (value == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla silindi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(Category category)
    {
        var value = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
        if (value == null)
        {
            return NotFound();
        }

        value.Name = category.Name;
        await _context.SaveChangesAsync();

        return Ok(value);
    }
}
