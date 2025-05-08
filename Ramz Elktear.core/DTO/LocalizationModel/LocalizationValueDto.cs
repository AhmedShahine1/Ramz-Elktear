using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.LocalizationModel
{
    public class LocalizationValueDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Culture code is required")]
        [StringLength(10, ErrorMessage = "Culture code must be no more than 10 characters")]
        public string CultureCode { get; set; }

        [Required(ErrorMessage = "Value is required")]
        public string Value { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
