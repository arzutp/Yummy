using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.ChefDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChefsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public ChefsController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _context.Chefs.ToListAsync();
        return Ok(_mapper.Map<List<ResultChefDto>>(values));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateChefDto createChefDto)
    {
        var chef = _mapper.Map<Chef>(createChefDto);
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
        return Ok(_mapper.Map<GetByIdChefDto>(value));
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
    public async Task<IActionResult> Update(UpdateChefDto updateChefDto)
    {
        var chef = _mapper.Map<Chef>(updateChefDto);
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
