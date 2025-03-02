using Ramz_Elktear.core.DTO.HomeModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IDashboardService
    {
        Task<HomePageViewModel> GetDashboardDataAsync();
    }
}
