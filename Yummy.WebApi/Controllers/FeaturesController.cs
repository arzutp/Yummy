using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.FeatureDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FeaturesController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public FeaturesController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var features = await _context.Features.ToListAsync();

        return Ok(_mapper.Map<List<ResultFeatureDto>>(features));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateFeatureDto createFeatureDto)
    {
        var value = _mapper.Map<Feature>(createFeatureDto);
        await _context.Features.AddAsync(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla eklendi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateFeatureDto updateFeatureDto)
    {
        var feature = _mapper.Map<Feature>(updateFeatureDto);

        _context.Features.Update(feature);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);

        if (value == null)
        {
            return NotFound("Böyle bir özellik bulunamadı");
        }

        _context.Features.Remove(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Features.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir özellik bulunamadı");
        }

        return Ok(_mapper.Map<GetByIdFeatureDto>(value));
    }
}
