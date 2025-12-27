namespace Yummy.WebUI.Dtos;

public class CreateAboutDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string VideoCoverImageUrl { get; set; }
    public string VideoUrl { get; set; }
    public string ReservationNumber { get; set; }
}
