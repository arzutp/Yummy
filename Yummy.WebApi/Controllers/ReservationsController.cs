using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.ReservationDtos;
using Yummy.WebApi.Entities;
using Yummy.WebApi.Enums;

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

    [HttpGet("GetTotalReservation")]
    public IActionResult GetTotalReservation()
    {
        var value =  _context.Reservations.Count();
        return Ok(value);
    }

    [HttpGet("GetPendingReservation")]
    public IActionResult GetPendingReservation()
    {
        var value = _context.Reservations.Where(x => x.Status == ReservationStatus.Wait).Count();
        return Ok(value);
    }

    [HttpGet("GetApprovedReservation")]
    public IActionResult GetApprovedReservation()
    {
        var value = _context.Reservations.Where(x => x.Status == ReservationStatus.Confirm).Count();
        return Ok(value);
    }

    [HttpGet("GetTotalCustomerCount")]
    public IActionResult GetTotalCustomerCount()
    {
        var value = _context.Reservations.Sum(x => x.CountOfPeople);
        return Ok(value);
    }

    [HttpGet("GetReservationStats")]
    public IActionResult GetReservationStats()
    {
        var today = DateTime.Today;

        var startOfThisMonth = new DateTime(today.Year, today.Month, 1);
        var startMonth = startOfThisMonth.AddMonths(-3);
        var endExclusive = startOfThisMonth.AddMonths(1);

        var rawData = _context.Reservations
            .Where(r => r.ReservationDate >= startMonth && r.ReservationDate < endExclusive)
            .GroupBy(r => new { r.ReservationDate.Year, r.ReservationDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Approved = g.Count(x => x.Status == ReservationStatus.Confirm),
                Canceled = g.Count(x => x.Status == ReservationStatus.Cancel)
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToList();

        var result = rawData.Select(x => new ReservationChartDto
        {
            Month = new DateTime(x.Year, x.Month, 1).ToString("MMM yyyy"),
            Approved = x.Approved,
            Canceled = x.Canceled
        }).ToList();

        return Ok(result);
    }

    [HttpGet("GetReservationSummaryLast4Months")]
    public IActionResult GetReservationSummaryLast4Months()
    {
        var today = DateTime.Today;

        var startOfThisMonth = new DateTime(today.Year, today.Month, 1);
        var startMonth = startOfThisMonth.AddMonths(-3);
        var endExclusive = startOfThisMonth.AddMonths(1);

        var query = _context.Reservations
            .Where(r => r.ReservationDate >= startMonth && r.ReservationDate < endExclusive);

        var approved = query.Count(x => x.Status == ReservationStatus.Confirm);
        var canceled = query.Count(x => x.Status == ReservationStatus.Cancel);

        var total = approved + canceled;

        int approvedPercent = total == 0 ? 0 : (int)Math.Round(approved * 100.0 / total);
        int canceledPercent = total == 0 ? 0 : (int)Math.Round(canceled * 100.0 / total);

        return Ok(new ReservationSummaryDto
        {
            ApprovedTotal = approved,
            CanceledTotal = canceled,
            ApprovedPercent = approvedPercent,
            CanceledPercent = canceledPercent
        });
    }
}
