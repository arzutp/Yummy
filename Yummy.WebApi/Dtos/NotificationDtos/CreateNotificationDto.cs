namespace Yummy.WebApi.Dtos.NotificationDtos;

public class CreateNotificationDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsRead { get; set; }
}
