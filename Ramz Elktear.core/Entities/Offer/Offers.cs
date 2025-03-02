using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;

namespace Ramz_Elktear.core.Entities.Offer
{
    public class Offers
    {
        public Offers()
        {
            CarOffers = new HashSet<CarOffer>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public decimal NewPrice { get; set; }
        public string ImageId { get; set; }
        public Images Image { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool delivery { get; set; }

        public ICollection<CarOffer> CarOffers { get; set; }

    }
}
