using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.AboutDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AboutsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public AboutsController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var abouts = await _context.Abouts.ToListAsync();

        return Ok(_mapper.Map<List<ResultAboutDto>>(abouts));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateAboutDto createAboutDto)
    {
        var value = _mapper.Map<About>(createAboutDto);
        await _context.Abouts.AddAsync(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla eklendi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateAboutDto updateAboutDto)
    {
        var About = _mapper.Map<About>(updateAboutDto);

        _context.Abouts.Update(About);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Abouts.FirstOrDefaultAsync(x => x.Id == id);

        if (value == null)
        {
            return NotFound("Böyle bir hakkında bilgisi bulunamadı");
        }

        _context.Abouts.Remove(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Abouts.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir hakkında bilgisi bulunamadı");
        }

        return Ok(_mapper.Map<GetByIdAboutDto>(value));
    }
}
