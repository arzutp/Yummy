using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Dtos.ChefEmployeeTaskDtos;

public class ResultChefEmployeeTaskDto
{
    public int Id { get; set; }

    public int ChefId { get; set; }
    public string ChefName { get; set; }
    public string ChefImageUrl { get; set; }

    public int EmployeeTaskId { get; set; }
    public string TaskName { get; set; }
    public Priority TaskPriority { get; set; }

    public DateTime AssignDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool TaskStatus { get; set; }
}

