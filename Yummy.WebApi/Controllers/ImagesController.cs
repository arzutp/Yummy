using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.ImageDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public ImagesController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var images = await _context.Images.ToListAsync();

        return Ok(_mapper.Map<List<ResultImageDto>>(images));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateImageDto createImageDto)
    {
        var image = _mapper.Map<Image>(createImageDto);

        _context.Images.Add(image);
        await _context.SaveChangesAsync();

        return Ok("Görsel başarıyla eklendi.");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateImageDto updateImageDto)
    {
        var image = _mapper.Map<Image>(updateImageDto);

        _context.Images.Update(image);
        await _context.SaveChangesAsync();

        return Ok("Görsel başarıyla güncellendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var image = await _context.Images.FindAsync(id);

        if (image == null)
        {
            return NotFound("Görsel bulunamadı.");
        }

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();

        return Ok("Görsel başarıyla silindi.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var image = await _context.Images.FindAsync(id);
        if (image == null)
        {
            return NotFound("Görsel bulunamadı.");
        }

        return Ok(_mapper.Map<ResultImageDto>(image));
    }
}
