using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.RoleModels
{
    public class RoleUserModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public IEnumerable<string> RoleId { get; set; }
    }

}
