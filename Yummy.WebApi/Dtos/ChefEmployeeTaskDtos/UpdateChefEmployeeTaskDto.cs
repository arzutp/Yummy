using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Dtos.ChefEmployeeTaskDtos;

public class UpdateChefEmployeeTaskDto
{
    public int Id { get; set; }
    public int ChefId { get; set; }
    public int EmployeeTaskId { get; set; }
    public DateTime AssignDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool TaskStatus { get; set; }
}
