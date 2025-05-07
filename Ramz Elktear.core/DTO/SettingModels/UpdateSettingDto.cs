using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.SettingModels
{
    public class UpdateSettingDto
    {
        public string Id { get; set; }
        public SettingType ImageType { get; set; }
        public IFormFile Image { get; set; }
    }
}
