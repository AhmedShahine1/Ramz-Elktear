using Ramz_Elktear.core.DTO.CarModels;

namespace Ramz_Elktear.core.DTO.OfferModels
{
    // Offer DTO
    public class OfferDetails
    {
        public string Id { get; set; }
        public CarDetails carDetails { get; set; }
        public decimal NewPrice { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool delivery { get; set; }
    }
}
