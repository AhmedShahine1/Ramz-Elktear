using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.CityModels;

namespace Ramz_Elktear.core.DTO.BookingModels
{
    public class BookingDto
    {
        public string Id { get; set; }
        public CityDto City { get; set; }
        public AuthDTO User { get; set; }
        public CarDetails Car { get; set; }
    }

}
