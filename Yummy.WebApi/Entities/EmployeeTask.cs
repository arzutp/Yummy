using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Entities;

public class EmployeeTask
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<ChefEmployeeTask> ChefEmployeeTasks { get; set; }
}
