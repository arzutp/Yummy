using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.ProductDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IValidator<Product> _validator;
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public ProductsController(IValidator<Product> productValidator, ApiContext context, IMapper mapper)
    {
        _validator = productValidator;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();

        return Ok(_mapper.Map<List<ResultProductDto>>(products));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto createProductDto)
    {
        var product = _mapper.Map<Product>(createProductDto);
        var validationResult = await _validator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return Ok("Ürün başarılı bir şekilde eklendi.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (value == null)
        {
            return NotFound("Ürün bulunamadı.");
        }

        _context.Products.Remove(value);
        await _context.SaveChangesAsync();

        return Ok("Ürün başarıyla silindi.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var value = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (value == null)
        {
            return NotFound("Ürün bulunamadı.");
        }

        return Ok(_mapper.Map<GetByIdProductDto>(value));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        var validationResult = _validator.Validate(product);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        _context.Update(product);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla güncellendi.");
    }

    [HttpGet("ProductWithCategory")]
    public async Task<IActionResult> GetProductWithCategory()
    {
        var products = await _context.Products.Include(x => x.Category).ToListAsync();
        return Ok(_mapper.Map<List<ProductsWithCategoryDto>>(products));
    }
}
