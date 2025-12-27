using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.TestimonialDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestimonialsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public TestimonialsController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _context.Testimonials.ToListAsync();
        return Ok(_mapper.Map<List<ResultTestimonialDto>>(values));
    }

    [HttpGet("GetAllWithPagination")]
    public async Task<IActionResult> GetAllWithPagination(int page = 1, int pageSize = 15)
    {
        var query = _context.Testimonials.AsNoTracking();

        var totalCount = query.Count();

        var testimonials = await query.OrderBy(x => x.Id)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

        var result = new PagedTestimonialResult
        {
            Items = _mapper.Map<List<ResultTestimonialDto>>(testimonials),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTestimonialDto createTestimonial)
    {
        var value = _mapper.Map<Testimonial>(createTestimonial);
        await _context.Testimonials.AddAsync(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla eklendi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateTestimonialDto updateTestimonial)
    {
        var value = _mapper.Map<Testimonial>(updateTestimonial);
        _context.Testimonials.Update(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Testimonials.FindAsync(id);
        if (value == null)
        {
            return NotFound("Kayıt bulunamadı");
        }
        _context.Testimonials.Remove(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Testimonials.FindAsync(id);
        if (value == null)
        {
            return NotFound("Kayıt bulunamadı");
        }
        return Ok(_mapper.Map<ResultTestimonialDto>(value));
    }
}
