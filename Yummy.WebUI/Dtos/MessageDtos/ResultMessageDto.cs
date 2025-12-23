namespace Yummy.WebUI.Dtos;

public class ResultMessageDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Detail { get; set; }
    public DateTime SendDate { get; set; }
    public bool IsRead { get; set; }
}
