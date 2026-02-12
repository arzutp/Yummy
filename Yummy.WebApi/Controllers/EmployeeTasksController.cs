using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.EmployeeTasksDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeTasksController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public EmployeeTasksController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeTaskDto createEmployeeTaskDto)
    {
        var employeeTask = _mapper.Map<EmployeeTask>(createEmployeeTaskDto);
        await _context.EmployeeTasks.AddAsync(employeeTask);
        await _context.SaveChangesAsync();

        return Ok(employeeTask);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employeeTasks = await _context.EmployeeTasks.ToListAsync();

        var result = _mapper.Map<List<ResultEmployeeTaskDto>>(employeeTasks);

        return Ok(result);
    }

    [HttpGet("GetAllWithPagination")]
    public async Task<IActionResult> GetAllWithPagination(int page = 1, int pageSize = 10)
    {
        var query = _context.EmployeeTasks.AsNoTracking();

        var totalCount = await query.CountAsync();

        var employeeTasks = await query
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedEmployeeTaskResult
        {
            Items = _mapper.Map<List<ResultEmployeeTaskDto>>(employeeTasks),
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
        var value = await _context.EmployeeTasks.FirstOrDefaultAsync(c => c.Id == id);

        if (value == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<GetByIdEmployeeTaskDto>(value));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.EmployeeTasks.FirstOrDefaultAsync(c => c.Id == id);
        if (value == null)
        {
            return NotFound();
        }

        _context.EmployeeTasks.Remove(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla silindi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateEmployeeTaskDto updateEmployeeTaskDto)
    {
        var value = await _context.EmployeeTasks.FirstOrDefaultAsync(c => c.Id == updateEmployeeTaskDto.Id);
        if (value == null)
        {
            return NotFound();
        }
        var EmployeeTask = _mapper.Map<EmployeeTask>(updateEmployeeTaskDto);
        value.Name = EmployeeTask.Name;
        await _context.SaveChangesAsync();

        return Ok(value);
    }
}
