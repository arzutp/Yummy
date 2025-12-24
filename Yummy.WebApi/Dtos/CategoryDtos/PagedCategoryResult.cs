namespace Yummy.WebApi.Dtos.CategoryDtos;

public class PagedCategoryResult
{
    public List<ResultCategoryDto> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
