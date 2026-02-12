using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Dtos.EmployeeTasksDtos;

public class CreateEmployeeTaskDto
{
    public string Name { get; set; }
    public Priority Priority { get; set; }
}
