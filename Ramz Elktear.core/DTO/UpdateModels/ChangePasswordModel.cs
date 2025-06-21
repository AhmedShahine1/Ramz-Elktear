using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.UpdateModels
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "UserIdRequired")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "CurrentPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "NewPasswordRequired")]
        [StringLength(100, ErrorMessage = "PasswordLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("NewPassword", ErrorMessage = "PasswordMismatch")]
        public string ConfirmPassword { get; set; }
    }
}
