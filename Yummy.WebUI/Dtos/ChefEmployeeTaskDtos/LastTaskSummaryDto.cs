using Yummy.WebUI.Enums;

namespace Yummy.WebUI.Dtos;

public class LastTaskSummaryDto
{
    public int Id { get; set; }
    public int EmployeeTaskId { get; set; }
    public string TaskName { get; set; }
    public DateTime AssignDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool TaskStatus { get; set; }
    public Priority TaskPriority { get; set; }
    public List<ResultChefDto> Chefs { get; set; } = new List<ResultChefDto>();
}
