using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.BrandModels
{
    public class UpdateBrandDto
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessage = "Arabic name is required")]
        [StringLength(100, ErrorMessage = "Arabic name cannot exceed 100 characters")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "English name is required")]
        [StringLength(100, ErrorMessage = "English name cannot exceed 100 characters")]
        public string NameEn { get; set; }

        [Required(ErrorMessage = "Arabic description is required")]
        [StringLength(500, ErrorMessage = "Arabic description cannot exceed 500 characters")]
        public string DescriptionAr { get; set; }

        [Required(ErrorMessage = "English description is required")]
        [StringLength(500, ErrorMessage = "English description cannot exceed 500 characters")]
        public string DescriptionEn { get; set; }

        // Optional image for update
        public IFormFile Image { get; set; }

        // Current image URL for display
        public string CurrentImageUrl { get; set; }

        // Flag to indicate if user wants to remove current image
        public bool RemoveCurrentImage { get; set; }
    }
}