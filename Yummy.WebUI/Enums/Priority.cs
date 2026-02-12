using System.ComponentModel.DataAnnotations;

namespace Yummy.WebUI.Enums;

public enum Priority
{
    [Display(Name = "Düşük")]
    Low,

    [Display(Name = "Orta")]
    Average,

    [Display(Name = "Yüksek")]
    High
}
