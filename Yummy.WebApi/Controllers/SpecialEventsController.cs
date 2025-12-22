using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.SpecialEventDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecialEventsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public SpecialEventsController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetSpecialEvents()
    {
        var values = await _context.SpecialEvents.ToListAsync();
        return Ok(_mapper.Map<List<ResultSpecialEventDto>>(values));
    }

    [HttpPost]
    public async Task<IActionResult> CreateSpecialEvent(CreateSpecialEventDto createSpecialEventDto)
    {
        var specialEvent = _mapper.Map<SpecialEvent>(createSpecialEventDto);
        await _context.SpecialEvents.AddAsync(specialEvent);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla eklendi");
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSpecialEvent(UpdateSpecialEventDto updateSpecialEventDto)
    {
        var specialEvent = _mapper.Map<SpecialEvent>(updateSpecialEventDto);
        _context.SpecialEvents.Update(specialEvent);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSpecialEvent(int id)
    {
        var specialEvent = await _context.SpecialEvents.FindAsync(id);
        if (specialEvent == null)
        {
            return NotFound();
        }
        _context.SpecialEvents.Remove(specialEvent);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecialEventById(int id)
    {
        var specialEvent = await _context.SpecialEvents.FindAsync(id);
        if (specialEvent == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<GetByIdSpecialEventDto>(specialEvent));
    }
}
