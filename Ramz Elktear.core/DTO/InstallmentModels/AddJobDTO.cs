using Ramz_Elktear.core.Helper;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.InstallmentModels
{
    public class AddJobDTO
    {
        [Required]
        public string Name { get; set; }

        public decimal Percentage { get; set; }
        public bool IsConvertable { get; set; }


        public string Description { get; set; }

        [Required]
        public JobSector Sector { get; set; } // Enum
    }

}
