using Microsoft.AspNetCore.Mvc.Rendering;
using Yummy.WebUI.Enums;

namespace Yummy.WebUI.Dtos;

public class UpdateChefEmployeeTaskDto
{
    public int Id { get; set; }
    public int ChefId { get; set; }
    public int EmployeeTaskId { get; set; }
    public DateTime AssignDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool TaskStatus { get; set; }
    public List<SelectListItem> Chefs { get; set; }
    public List<SelectListItem> Tasks { get; set; }
}
