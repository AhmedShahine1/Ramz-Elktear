using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.Entities.Booking
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public ApplicationUser Buyer { get; set; }

        public ApplicationUser Seller { get; set; }

        public Car Car { get; set; }

        public string Status { get; set; } = BookingStatus.send.ToString();

        public DateTime CreateAt { get; set; } = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Arabian Standard Time");
    }
}
