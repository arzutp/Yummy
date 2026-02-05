using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Entities;

public class ChefEmployeeTask
{
    public int ChefId { get; set; }
    public Chef Chef { get; set; }

    public int EmployeeTaskId { get; set; }
    public EmployeeTask EmployeeTasks { get; set; }

    public DateTime AssignDate { get; set; }
    public DateTime DueDate { get; set; }
    public Priority TaskStatus { get; set; }
    public string Priority { get; set; }
}
