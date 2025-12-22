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
