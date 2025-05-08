using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.LocalizationModel
{
    public class LocalizationResourceDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Resource key is required")]
        [StringLength(255, ErrorMessage = "Resource key must be no more than 255 characters")]
        public string ResourceKey { get; set; }

        [StringLength(100, ErrorMessage = "Resource group must be no more than 100 characters")]
        public string ResourceGroup { get; set; }

        [StringLength(500, ErrorMessage = "Description must be no more than 500 characters")]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<LocalizationValueDto> Values { get; set; } = new List<LocalizationValueDto>();
    }
}
