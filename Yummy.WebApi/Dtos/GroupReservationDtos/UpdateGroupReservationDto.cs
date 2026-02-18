using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Dtos;

public class UpdateGroupReservationDto
{
    public int Id { get; set; }
    public string ResponsibleCustomerName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public int PersonCount { get; set; }
    public string GroupTitle { get; set; }
    public string Details { get; set; }
    public DateTime ReservationDate { get; set; }
    public DateTime LastProcessDate { get; set; }
    public ReservationStatus Status { get; set; }
    public Priority Priority { get; set; }
}
