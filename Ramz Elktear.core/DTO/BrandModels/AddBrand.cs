using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.BrandModels
{
    public class AddBrand
    {
        public string Name { get; set; }
        public IFormFile Logo { get; set; }
    }
}
