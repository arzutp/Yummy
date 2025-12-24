using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.ServiceDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public ServicesController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var servies = await _context.Services.ToListAsync();

        return Ok(_mapper.Map<List<ResultServiceDto>>(servies));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateServiceDto createServiceDto)
    {
        var value = _mapper.Map<Service>(createServiceDto);
        await _context.Services.AddAsync(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla eklendi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateServiceDto updateServiceDto)
    {
        var value = _mapper.Map<Service>(updateServiceDto);
        _context.Services.Update(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir servis bulunamadı");
        }
        _context.Services.Remove(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir servis bulunamadı");
        }
        return Ok(_mapper.Map<ResultServiceDto>(value));
    }
}
