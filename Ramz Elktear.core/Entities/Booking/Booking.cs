using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Cars;

namespace Ramz_Elktear.core.Entities.Booking
{
    public class Booking
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public City City { get; set; }

        public ApplicationUser User { get; set; }

        public Car Car { get; set; }
    }
}
