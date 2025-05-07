using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.HomeModels
{
    public class BookingStatsByDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int BookingCount { get; set; }
        public BookingStatus Status { get; set; }
    }
}
