using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.UpdateModels
{
    public class UpdatePhoneNumberModel
    {
        [Required(ErrorMessage = "UserIdRequired")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "PhoneNumberRequired")]
        [Phone(ErrorMessage = "InvalidPhoneFormat")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
