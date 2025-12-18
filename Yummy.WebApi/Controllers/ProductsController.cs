using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Yummy.WebApi.Context;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IValidator<Product> _validator;
    private readonly ApiContext _context;

    public ProductsController(IValidator<Product> productValidator, ApiContext context)
    {
        _validator = productValidator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _context.Products.ToListAsync();

        return Ok(products);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        var validationResult = await _validator.ValidateAsync(product);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return Ok("Ürün başarılı bir şekilde eklendi.");
    }

    [HttpDelete("id")]
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

    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        var value = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (value == null)
        {
            return NotFound("Ürün bulunamadı.");
        }

        return Ok(value);
    }

    [HttpPut]
    public async Task<IActionResult> Update(Product product)
    {
        var validationResult = _validator.Validate(product);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        _context.Update(product);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla güncellendi.");
    }
}
