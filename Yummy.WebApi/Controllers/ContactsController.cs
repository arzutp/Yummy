using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yummy.WebApi.Context;
using Yummy.WebApi.Dtos.ContactDtos;
using Yummy.WebApi.Entities;

namespace Yummy.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly ApiContext _context;

    public ContactsController(ApiContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var values = await _context.Contacts.ToListAsync();

        return Ok(values);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateContactDto createContactDto)
    {
        Contact contact = new();
        contact.MapLocation = createContactDto.MapLocation;
        contact.Address = createContactDto.Address;
        contact.Phone = createContactDto.Phone;
        contact.Email = createContactDto.Email;
        contact.OpenHours = createContactDto.OpenHours;
        await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();

        return Ok(contact);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        if (contact == null)
        {
            return NotFound();
        }
        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
        return Ok("Başarıyla silindi");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var value = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        if (value == null)
        {
            return NotFound();
        }
        return Ok(value);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateContactDto updateContactDto)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == updateContactDto.Id);
        if (contact == null)
        {
            return NotFound();
        }
        contact.MapLocation = updateContactDto.MapLocation;
        contact.Address = updateContactDto.Address;
        contact.Phone = updateContactDto.Phone;
        contact.Email = updateContactDto.Email;
        contact.OpenHours = updateContactDto.OpenHours;
        await _context.SaveChangesAsync();
        return Ok(contact);
    }
}
