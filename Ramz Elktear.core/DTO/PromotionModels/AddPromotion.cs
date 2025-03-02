using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.PromotionModels
{
    public class AddPromotion
    {
        public IFormFile ImageAr {  get; set; }
        public IFormFile ImageEn {  get; set; }
        public string redirctURL { get; set; }
    }
}
