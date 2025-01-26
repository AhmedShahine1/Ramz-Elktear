using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string PhoneNumber { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
