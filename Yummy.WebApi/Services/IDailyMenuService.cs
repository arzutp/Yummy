using Yummy.WebApi.Dtos.DailyMenusDtos;

namespace Yummy.WebApi.Services;

public interface IDailyMenuService
{
    Task<DailyMenuDto> GetDailyMenuAsync();
}
