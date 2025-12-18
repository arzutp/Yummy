namespace Yummy.WebApi.Entities;

public class Reservation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime ReservationDate { get; set; }
    public string ReservationTime { get; set; }
    public int CountOfPeople { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
}
