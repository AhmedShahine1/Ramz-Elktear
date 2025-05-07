using Ramz_Elktear.core.Entities.Cars;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.Entities.Offer
{
    public class CarOffer
    {
        public string OfferId { get; set; }
        public Offers Offer { get; set; }

        [ForeignKey("Car")]
        public string CarId { get; set; }
        public Car Car { get; set; }

        public decimal SellingPrice { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
