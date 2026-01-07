using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.ReservationDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly IMapper _mapper;

    public ReservationsController(ApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10)
    {
        var query = _context.Reservations.AsNoTracking();
        var totalCount = query.Count();

        var reservations = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var reservationsDto = _mapper.Map<List<ResultReservationDto>>(reservations);

        var pagedReservastionResult = new PagedReservastionResult
        {
            Items = reservationsDto,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

        return Ok(pagedReservastionResult);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ResultReservationDto>(reservation));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationDto createReservationDto)
    {
        var reservation = _mapper.Map<Reservation>(createReservationDto);

        _context.Reservations.Add(reservation);

        await _context.SaveChangesAsync();

        return Ok("Rezervasyon başarıyla oluşturuldu");
    }

    [HttpPut]
    public async Task<IActionResult> Update(ResultReservationDto resultReservationDto)
    {
        var reservation = await _context.Reservations.FindAsync(resultReservationDto.Id);

        if (reservation == null)
        {
            return NotFound();
        }
        _mapper.Map(resultReservationDto, reservation);

        await _context.SaveChangesAsync();

        return Ok("Rezervasyon başarıyla güncellendi");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }
        _context.Reservations.Remove(reservation);

        await _context.SaveChangesAsync();

        return Ok("Rezervasyon başarıyla silindi");
    }
}
