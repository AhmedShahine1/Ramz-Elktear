using Ramz_Elktear.core.DTO.HomeModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IDashboardService
    {
        Task<HomePageViewModel> GetDashboardDataAsync();
        Task<ManagerDashboardDto> GetManagerDashboardData(string managerId);
        Task<SalesDashboardDto> GetSalesDashboardData(string salesId);
    }
}
