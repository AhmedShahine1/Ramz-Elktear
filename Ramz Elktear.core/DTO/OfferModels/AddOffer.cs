using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.OfferModels
{
    // Offer DTO
    public class AddOffer
    {
        public string CarId { get; set; }
        public decimal NewPrice { get; set; }
        public IFormFile Image { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool delivery { get; set; }
    }
}
