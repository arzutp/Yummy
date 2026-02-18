using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GroupReservationsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public GroupReservationsController(ApiContext apiContext, IMapper mapper)
    {
        _context = apiContext;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _context.GroupReservations.ToListAsync();

        return Ok(_mapper.Map<List<ResultGroupReservationDto>>(values));
    }

    [HttpGet("GetTodayReservation")]
    public async Task<IActionResult> GetTodayReservation()
    {
        var values = await _context.GroupReservations.Where(x => x.ReservationDate.Date == DateTime.Now.Date).ToListAsync();

        return Ok(_mapper.Map<List<ResultGroupReservationDto>>(values));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGroupReservationDto createGroupReservationDto)
    {
        var value = _mapper.Map<GroupReservation>(createGroupReservationDto);
        await _context.GroupReservations.AddAsync(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla eklendi");
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateGroupReservationDto updateGroupReservationDto)
    {
        var value = _mapper.Map<GroupReservation>(updateGroupReservationDto);
        value.LastProcessDate = DateTime.Now;
        _context.GroupReservations.Update(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var value = await _context.GroupReservations.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir rezervasyon bulunamadı");
        }
        _context.GroupReservations.Remove(value);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.GroupReservations.FirstOrDefaultAsync(x => x.Id == id);
        if (value == null)
        {
            return NotFound("Böyle bir rezervasyon bulunamadı");
        }
        return Ok(_mapper.Map<GetByIdGroupReservationDto>(value));
    }
}
