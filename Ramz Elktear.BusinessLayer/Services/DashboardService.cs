using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.DTO.HomeModels;
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
            var bookings = await _unitOfWork.BookingRepository.GetAllAsync(include:q=>q.Include(a=>a.Car).ThenInclude(a=>a.Brand));
            var totalCars = await _unitOfWork.CarRepository.CountAsync();
            int totalBookings = bookings.Count();

            // Bookings by Month
            var bookingStatsByMonth = bookings
                .GroupBy(b => new { b.CreateAt.Year, b.CreateAt.Month })
                .Select(group => new BookingStatsByMonthDto
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    BookingCount = group.Count()
                })
                .ToList();

            // Bookings by Brand
            var bookingStatsByBrand = bookings
                .GroupBy(b => b.Car.Brand.NameEn)
                .Select(group => new BrandBookingStats
                {
                    BrandName = group.Key,
                    TotalBookings = group.Count()
                })
                .ToList();

            // Bookings by Brand Over Time
            var brandBookingsOverTime = bookings
                .GroupBy(b => new { b.CreateAt.Year, b.CreateAt.Month, b.Car.Brand.NameEn })
                .Select(group => new BrandBookingByMonthDto
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    BrandName = group.Key.NameEn,
                    BookingCount = group.Count()
                })
                .ToList();

            return new HomePageViewModel
            {
                TotalBookings = totalBookings,
                TotalCars = totalCars,
                BookingStatsByMonth = bookingStatsByMonth,
                BookingStatsByBrand = bookingStatsByBrand,
                BrandBookingsOverTime = brandBookingsOverTime
            };
        }
    }
}
