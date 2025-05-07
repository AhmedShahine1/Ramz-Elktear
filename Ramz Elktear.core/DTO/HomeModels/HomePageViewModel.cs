using Ramz_Elktear.core.DTO.BookingModels;
using System.Collections.Generic;

namespace Ramz_Elktear.core.DTO.HomeModels
{
    public class HomePageViewModel
    {
        public int TotalBookings { get; set; }
        public int TotalCars { get; set; }
        public List<BookingStatsByDate> BookingStatsByDate { get; set; }
        public List<BrandBookingStats> BookingStatsByBrand { get; set; }
        public List<BrandBookingByMonthDto> BrandBookingsOverTime { get; set; }
        public List<BookingStatusCountDto> BookingCountByStatus { get; set; }
    }
}
