namespace Yummy.WebApi.Dtos.ChefEmployeeTaskDtos;

public class PagedChefEmployeeTaskResult
{
    public List<ResultChefEmployeeTaskDto> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
