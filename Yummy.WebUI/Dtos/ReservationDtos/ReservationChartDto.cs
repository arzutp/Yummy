namespace Yummy.WebUI.Dtos;

public class ReservationChartDto
{
    public string Month { get; set; } = default!;
    public int Approved { get; set; }
    public int Canceled { get; set; }
}
