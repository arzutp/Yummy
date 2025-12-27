namespace Yummy.WebUI.Dtos;

public class UpdateSpecialEventDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool Status { get; set; }
    public decimal Price { get; set; }
}
