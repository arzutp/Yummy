namespace Yummy.WebApi.Dtos.NotificationDtos;

public class PagedNotificationResult
{
    public List<ResultNotificationDto> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
