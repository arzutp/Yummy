using Yummy.WebApi.Enums;

namespace Yummy.WebApi.Dtos.EmployeeTasksDtos;

public class ResultEmployeeTaskDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Priority Priority { get; set; }
}
