using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.OriginModels
{
    public class AddOrigin
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool IsActive { get; set; }
    }
}
