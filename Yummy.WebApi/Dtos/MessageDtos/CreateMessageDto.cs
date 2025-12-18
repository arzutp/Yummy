namespace Yummy.WebApi.Dtos.MessageDtos;

public class CreateMessageDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Detail { get; set; }
    public bool IsRead { get; set; }
}
