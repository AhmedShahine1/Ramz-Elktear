using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.OfferModels
{
    public class AddOffer
    {
        public decimal NewPrice { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public IFormFile Image { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CreatedBy { get; set; }
        public bool Delivery { get; set; }
    }
}
