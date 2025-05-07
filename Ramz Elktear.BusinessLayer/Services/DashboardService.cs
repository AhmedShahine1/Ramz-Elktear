using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.DTO.HomeModels;
using Ramz_Elktear.core.Helper;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HomePageViewModel> GetDashboardDataAsync()
        {
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(
                include: q => q.Include(a => a.Car).ThenInclude(a => a.Brand)
            );

            var totalCars = await _unitOfWork.CarRepository.CountAsync();
            var totalBookings = bookings.Count();

            var bookingStatsByDate = bookings
                .Select(b => new BookingStatsByDate
                {
                    Year = b.CreateAt.Year,
                    Month = b.CreateAt.Month,
                    Day = b.CreateAt.Day,
                    BookingCount = 1,
                    Status = Enum.Parse<BookingStatus>(b.Status)
                })
                .ToList();

            var bookingStatsByBrand = bookings
                .GroupBy(b => b.Car.Brand.NameEn)
                .Select(g => new BrandBookingStats
                {
                    BrandName = g.Key,
                    TotalBookings = g.Count()
                })
                .ToList();

            var brandBookingsOverTime = bookings
                .GroupBy(b => new { b.CreateAt.Year, b.CreateAt.Month, b.Car.Brand.NameEn })
                .Select(g => new BrandBookingByMonthDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    BrandName = g.Key.NameEn,
                    BookingCount = g.Count()
                })
                .ToList();

            var bookingCountByStatus = bookings
                .GroupBy(b => b.Status)
                .Select(g => new BookingStatusCountDto
                {
                    Status = Enum.Parse<BookingStatus>(g.Key),
                    Count = g.Count()
                })
                .ToList();

            return new HomePageViewModel
            {
                TotalBookings = totalBookings,
                TotalCars = totalCars,
                BookingStatsByDate = bookingStatsByDate,
                BookingStatsByBrand = bookingStatsByBrand,
                BrandBookingsOverTime = brandBookingsOverTime,
                BookingCountByStatus = bookingCountByStatus
            };
        }

        public async Task<ManagerDashboardDto> GetManagerDashboardData(string managerId)
        {
            var salesAgents = await _unitOfWork.UserRepository.FindAllAsync(
                u => u.ManagerId == managerId ,
                isNoTracking: true
            );
            var salesIds = salesAgents.Select(s => s.Id).ToList();

            var bookings = await _unitOfWork.BookingRepository.FindAllAsync(
                b => salesIds.Contains(b.Seller.Id),
                include: a => a.Include(q => q.Seller),
                isNoTracking: true
            );

            var monthlySales = bookings.AsEnumerable()
                .GroupBy(b => new { b.CreateAt.Year, b.CreateAt.Month })
                .Select(g => new MonthlySalesDto
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    SalesCount = g.Count()
                })
                .OrderBy(m => m.Year).ThenBy(m => m.Month)
                .ToList();

            var salesComparison = salesAgents.Select(s => new SalesPerformanceDto
            {
                SalesId = s.Id,
                SalesName = s.UserName,
                TotalBookings = bookings.Count(b => b.Seller.Id == s.Id)
            }).ToList();

            return new ManagerDashboardDto
            {
                TotalSalesAgents = salesAgents.Count(),
                TotalSales = bookings.Count(),
                MonthlySales = monthlySales,
                SalesComparison = salesComparison
            };
        }

        public async Task<SalesDashboardDto> GetSalesDashboardData(string salesId)
        {
            var bookings = await _unitOfWork.BookingRepository.FindAllAsync(
                u => u.Seller.Id == salesId,
                isNoTracking: true
            );

            var monthlySales = bookings.AsEnumerable()
                .GroupBy(b => new { b.CreateAt.Year, b.CreateAt.Month })
                .Select(g => new MonthlySalesDto
                {
                    Month = g.Key.Month,
                    Year = g.Key.Year,
                    SalesCount = g.Count()
                })
                .OrderBy(m => m.Year).ThenBy(m => m.Month)
                .ToList();

            return new SalesDashboardDto
            {
                TotalSales = bookings.Count(),
                MonthlySales = monthlySales
            };
        }
    }
}