using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ramz_Elktear.core.Entities.Offer
{
    public class Offer
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CarId { get; set; }
        public Car Car { get; set; }
        public decimal NewPrice { get; set; }
        public string ImageId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiryDate { get; set; }
        public bool delivery { get; set; }
    }
}
