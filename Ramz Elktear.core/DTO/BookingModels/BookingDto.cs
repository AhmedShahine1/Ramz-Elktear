using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.CityModels;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.BookingModels
{
    public class BookingDto
    {
        public string Id { get; set; }
        public string BookingStatus { get; set; }
        public string BookingStatusDisplay => Enum.TryParse<BookingStatus>(BookingStatus, out var status)
                                      ? status.GetDisplayName()
                                      : BookingStatus;
        public AuthDTO Buyer { get; set; }
        public AuthDTO Seller { get; set; }
        public CarDTO Car { get; set; }
        public DateTime CreateAt { get; set; }
    }
}