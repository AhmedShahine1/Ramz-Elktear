using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.LocalizationModel
{

    public class LocalizationExportRequest
    {
        [Required]
        public string Format { get; set; } = "json";
        public string Group { get; set; }
    }
}
