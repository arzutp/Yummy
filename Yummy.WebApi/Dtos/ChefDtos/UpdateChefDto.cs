namespace Yummy.WebApi.Dtos.ChefDtos;

public class UpdateChefDto
{
    public int Id { get; set; }
    public string NameSurname { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
}
