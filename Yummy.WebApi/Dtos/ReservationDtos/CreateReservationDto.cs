using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Dtos.ReservationDtos;

public class CreateReservationDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime ReservationDate { get; set; }
    public int CountOfPeople { get; set; }
    public string Message { get; set; }
    public ReservationStatus Status { get; set; }
}
