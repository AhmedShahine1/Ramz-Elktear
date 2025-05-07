namespace Ramz_Elktear.core.DTO.HomeModels
{
    public class ManagerDashboardDto
    {
        public int TotalSalesAgents { get; set; }
        public int TotalSales { get; set; }
        public List<MonthlySalesDto> MonthlySales { get; set; }
        public List<SalesPerformanceDto> SalesComparison { get; set; }
    }
}
