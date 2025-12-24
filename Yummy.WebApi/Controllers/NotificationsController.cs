using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.NotificationDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;
    public NotificationsController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _context.Notifications.ToListAsync();
        return Ok(_mapper.Map<List<ResultNotificationDto>>(values));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateNotificationDto dto)
    {
        var notification = _mapper.Map<Notification>(dto);
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla eklendi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateNotificationDto dto)
    {
        var value = _mapper.Map<Notification>(dto);

        _context.Notifications.Update(value);
        await _context.SaveChangesAsync();

        return Ok("Başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir bildirim bulunamadı");
        }
        _context.Notifications.Remove(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir bildirim bulunamadı");
        }
        return Ok(_mapper.Map<ResultNotificationDto>(value));
    }
}
