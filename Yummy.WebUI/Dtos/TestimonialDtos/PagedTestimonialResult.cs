namespace Yummy.WebUI.Dtos;

public class PagedTestimonialResult
{
    public List<ResultTestimonialDto> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}