using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.ColorModels
{
    public class AddColor
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
    }
}
