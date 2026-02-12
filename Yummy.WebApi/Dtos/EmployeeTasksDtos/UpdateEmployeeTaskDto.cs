using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Dtos.EmployeeTasksDtos;

public class UpdateEmployeeTaskDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Priority Priority { get; set; }
}
