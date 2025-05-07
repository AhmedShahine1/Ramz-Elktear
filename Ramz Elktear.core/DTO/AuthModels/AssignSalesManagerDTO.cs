using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.AuthModels
{
    public class AssignSalesManagerDTO
    {
        [Required]
        public string SalesId { get; set; }

        [Required]
        public string ManagerId { get; set; }
    }
}
