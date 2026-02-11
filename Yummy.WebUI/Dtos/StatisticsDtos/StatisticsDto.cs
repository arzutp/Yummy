using System.ComponentModel.DataAnnotations;

namespace Yummy.WebUI.Dtos;

public class StatisticsDto
{
    [Display(Name = "Ürün Sayısı")]
    public int ProductCount { get; set; }

    [Display(Name = "Şef Sayısı")]
    public int ChefCount { get; set; }

    [Display(Name = "Rezervasyon Sayısı")]
    public int ReservationCount { get; set; }

    [Display(Name = "Toplam Müşteri Sayısı")]
    public int TotalGuestCount { get; set; }
}
