using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.CategoryModels
{
    public class AddCategoryDTO
    {
        [Required]
        public string NameAr { get; set; }
        [Required]
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? Image { get; set; }
    }
}
