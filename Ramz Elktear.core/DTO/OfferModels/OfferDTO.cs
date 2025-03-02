using Ramz_Elktear.core.DTO.CarModels;

namespace Ramz_Elktear.core.DTO.OfferModels
{
    public class OfferDTO
    {
        public string Id { get; set; }
        public CarDTO CarDetails { get; set; }
        public decimal NewPrice { get; set; }
        public string ImageUrl { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool Delivery { get; set; }
    }
}
