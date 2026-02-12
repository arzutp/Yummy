using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.ChefDtos;
using Yummy.WebApi.Dtos.ChefEmployeeTaskDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChefEmployeeTasksController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public ChefEmployeeTasksController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateChefEmployeeTaskDto createChefEmployeeTaskDto)
    {
        var chefEmployeeTask = _mapper.Map<ChefEmployeeTask>(createChefEmployeeTaskDto);
        chefEmployeeTask.AssignDate = DateTime.Now;
        await _context.ChefEmployeeTasks.AddAsync(chefEmployeeTask);
        await _context.SaveChangesAsync();

        return Ok(chefEmployeeTask);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var chefEmployeeTasks = await _context.ChefEmployeeTasks.ToListAsync();

        var result = _mapper.Map<List<ResultChefEmployeeTaskDto>>(chefEmployeeTasks);

        return Ok(result);
    }

    [HttpGet("GetLast10Tasks")]
    public async Task<IActionResult> GetLast10Tasks()
    {
        var data = await _context.ChefEmployeeTasks
        .Include(x => x.Chef)
        .Include(x => x.EmployeeTasks)
        .OrderByDescending(x => x.AssignDate)
        .ToListAsync();

        var lastTaskSummary = data
            .GroupBy(x => x.EmployeeTaskId)
            .Select(g =>
            {
                var last = g.OrderByDescending(x => x.AssignDate).First();

                return new LastTaskSummaryDto
                {
                    EmployeeTaskId = last.EmployeeTaskId,
                    TaskName = last.EmployeeTasks.Name,
                    AssignDate = last.AssignDate,
                    DueDate = last.DueDate,
                    TaskStatus = last.TaskStatus,
                    TaskPriority = last.EmployeeTasks.Priority,
                    Chefs = g.Select(c => new ResultChefDto
                    {
                        Id = c.Chef.Id,
                        NameSurname = c.Chef.NameSurname,
                        ImageUrl = c.Chef.ImageUrl
                    }).ToList()
                };
            })
            .OrderByDescending(x => x.AssignDate)
            .Take(10)
            .ToList();

        return Ok(lastTaskSummary);
    }

    [HttpGet("GetAllWithPagination")]
    public async Task<IActionResult> GetAllWithPagination(int page = 1, int pageSize = 10)
    {
        var query = _context.ChefEmployeeTasks.AsNoTracking();

        var totalCount = await query.CountAsync();

        var chefEmployeeTasks = await query
            .Include(x => x.EmployeeTasks)
            .Include(x => x.Chef)
            .OrderByDescending(x => x.AssignDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedChefEmployeeTaskResult
        {
            Items = _mapper.Map<List<ResultChefEmployeeTaskDto>>(chefEmployeeTasks),
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
        var value = await _context.ChefEmployeeTasks.FirstOrDefaultAsync(c => c.Id == id);

        if (value == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<GetByIdChefEmployeeTaskDto>(value));
    }

    [HttpGet("GetByTaskId/{id}")]
    public async Task<IActionResult> GetByTaskId(int id)
    {
        var data = await _context.ChefEmployeeTasks
        .Include(x => x.Chef)
        .Include(x => x.EmployeeTasks)
        .OrderByDescending(x => x.AssignDate)
        .ToListAsync();

        var lastTaskSummary = data
            .GroupBy(x => x.EmployeeTaskId)
            .Select(g =>
            {
                var last = g.OrderByDescending(x => x.AssignDate).First();

                return new LastTaskSummaryDto
                {
                    EmployeeTaskId = last.EmployeeTaskId,
                    TaskName = last.EmployeeTasks.Name,
                    AssignDate = last.AssignDate,
                    DueDate = last.DueDate,
                    TaskStatus = last.TaskStatus,
                    TaskPriority = last.EmployeeTasks.Priority,
                    Chefs = g.Select(c => new ResultChefDto
                    {
                        Id = c.Chef.Id,
                        NameSurname = c.Chef.NameSurname,
                        ImageUrl = c.Chef.ImageUrl
                    }).ToList()
                };
            }).FirstOrDefault();

        return Ok(lastTaskSummary);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.ChefEmployeeTasks.FirstOrDefaultAsync(c => c.Id == id);
        if (value == null)
        {
            return NotFound();
        }

        _context.ChefEmployeeTasks.Remove(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla silindi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateChefEmployeeTaskDto dto)
    {
        var entity = await _context.ChefEmployeeTasks
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (entity == null)
        {
            return NotFound();
        }

        entity.DueDate = dto.DueDate;
        entity.TaskStatus = dto.TaskStatus;
        entity.ChefId = dto.ChefId;
        entity.EmployeeTaskId = dto.EmployeeTaskId;

        await _context.SaveChangesAsync();

        return Ok();
    }

}
