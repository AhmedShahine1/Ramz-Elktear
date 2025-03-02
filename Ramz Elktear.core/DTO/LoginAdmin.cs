using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO
{
    public class LoginAdmin
    {
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
