using System.ComponentModel.DataAnnotations;

namespace Yummy.WebUI.Enums;

public enum ReservationStatus
{
    [Display(Name = "Onay Bekleniyor")]
    Wait,

    [Display(Name = "Onaylandı")]
    Confirm,

    [Display(Name = "İptal Edildi")]
    Cancel
}