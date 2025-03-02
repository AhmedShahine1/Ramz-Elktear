using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.TransmissionTypeModels
{
    public class AddTransmissionType
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public IFormFile? Image { get; set; }  // For image upload
        public bool IsActive { get; set; }
    }
}