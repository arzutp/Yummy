namespace Yummy.WebApi.Dtos.SpecialEventDtos;

public class GetByIdSpecialEventDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public bool Status { get; set; }
    public decimal Price { get; set; }
}
