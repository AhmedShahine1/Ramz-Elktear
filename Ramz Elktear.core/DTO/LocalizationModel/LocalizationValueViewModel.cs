using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.LocalizationModel
{
    public class LocalizationValueViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Culture code is required")]
        [StringLength(10, ErrorMessage = "Culture code must be no more than 10 characters")]
        public string CultureCode { get; set; }

        public string Value { get; set; }
    }
}
