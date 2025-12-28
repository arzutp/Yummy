using Yummy.WebUI.Enums;

namespace Yummy.WebUI.Dtos;

public class ResultReservationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime ReservationDate { get; set; }
    public int CountOfPeople { get; set; }
    public string Message { get; set; }
    public ReservationStatus Status { get; set; }
}
