using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.BrandModels
{
    public class UpdateBrandDto
    {
        public string Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public IFormFile Image { get; set; }
    }
}
