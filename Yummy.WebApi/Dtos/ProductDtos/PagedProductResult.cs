namespace Yummy.WebApi.Dtos.ProductDtos;

public class PagedProductResultDto
{
    public List<ResultProductDto> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
