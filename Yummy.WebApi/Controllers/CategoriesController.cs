using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.CategoryDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public CategoriesController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
    {
        var category = _mapper.Map<Category>(createCategoryDto);
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _context.Categories.ToListAsync();

        var result = _mapper.Map<List<ResultCategoryDto>>(categories);

        return Ok(result);
    }

    [HttpGet("GetAllWithPagination")]
    public async Task<IActionResult> GetAllWithPagination(int page = 1, int pageSize = 10)
    {
        var query = _context.Categories.AsNoTracking();

        var totalCount = await query.CountAsync();

        var categories = await query
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedCategoryResult
        {
            Items = _mapper.Map<List<ResultCategoryDto>>(categories),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

        if (value == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<GetByIdCategoryDto>(value));
    }

    [HttpDelete("{id}")]
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
    public async Task<IActionResult> Update(UpdateCategoryDto updateCategoryDto)
    {
        var value = await _context.Categories.FirstOrDefaultAsync(c => c.Id == updateCategoryDto.Id);
        if (value == null)
        {
            return NotFound();
        }
        var category = _mapper.Map<Category>(updateCategoryDto);
        value.Name = category.Name;
        await _context.SaveChangesAsync();

        return Ok(value);
    }
}
