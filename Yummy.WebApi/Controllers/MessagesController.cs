using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.MessageDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessagesController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public MessagesController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _context.Messages.ToListAsync();

        return Ok(_mapper.Map<List<ResultMessageDto>>(values));
    }

    [HttpGet("GetAllWithPagination")]
    public async Task<IActionResult> GetAllWithPagination(int page = 1, int pageSize = 10)
    {
        var query = _context.Messages.AsNoTracking();

        var totalCount = await query.CountAsync();

        var values = await query.OrderBy(x => x.Id)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize).ToListAsync();

        var result = new PagedMessageResult
        {
            Items = _mapper.Map<List<ResultMessageDto>>(values),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };  

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateMessageDto createMessageDto)
    {
        var value = _mapper.Map<Message>(createMessageDto);
        value.SendDate = DateTime.UtcNow;
        await _context.Messages.AddAsync(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla eklendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);

        if (value == null)
        {
            return NotFound("Böyle bir mesaj bulunamadı");
        }

        _context.Messages.Remove(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir mesaj bulunamadı");
        }

        return Ok(_mapper.Map<GetByIdMessageDto>(value));
    }

    [HttpGet("GetUnreadMesssages")]
    public async Task<IActionResult> GetUnreadMessageList()
    {
        var values = await _context.Messages.Where(x => x.IsRead == false).ToListAsync();
        return Ok(_mapper.Map<List<ResultMessageDto>>(values));
    }
}
