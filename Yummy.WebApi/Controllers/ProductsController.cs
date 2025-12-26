using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
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
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
    {
        var query = await _context.Products.ToListAsync();

        var result = _mapper.Map<List<ResultProductDto>>(query);

        return Ok(result);
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
    public async Task<IActionResult> GetProductWithCategory(int page = 1, int pageSize = 10)
    {
        var query = _context.Products.AsNoTracking();
        var totalCount = await query.CountAsync();

        var products = await query.Include(x => x.Category).OrderBy(x => x.Id).
                                Skip((page - 1) * pageSize).
                                Take(pageSize)
                                .ToListAsync();

        var result = new PagedProductWithCategoryResultDto
        {
            Items = _mapper.Map<List<ProductsWithCategoryDto>>(products),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return Ok(result);
    }
}
