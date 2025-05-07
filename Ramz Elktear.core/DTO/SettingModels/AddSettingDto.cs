using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.SettingModels
{
    public class AddSettingDto
    {
        public SettingType ImageType { get; set; }  // Example: "Login", "Register", "Logo"
        public IFormFile Image { get; set; }
    }
}
