using Microsoft.AspNetCore.Mvc.Rendering;

namespace Yummy.WebUI.Dtos;

public class CreateProductWithCategoriesDto
{
    public List<SelectListItem> Categories { get; set; }
}
