namespace Yummy.WebApi.Dtos.ReservationDtos;

public class ReservationSummaryDto
{
    public int ApprovedTotal { get; set; }
    public int CanceledTotal { get; set; }
    public int ApprovedPercent { get; set; }
    public int CanceledPercent { get; set; }
}
