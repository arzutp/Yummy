namespace Yummy.WebUI.Dtos;

public class UpdateNotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string IconUrl { get; set; }
    public DateTime DateTime { get; set; }
    public bool IsRead { get; set; }
}
