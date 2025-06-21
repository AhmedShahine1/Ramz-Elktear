using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.UpdateModels
{
    public class UpdateUserDetails
    {
        [Required(ErrorMessage = "UserIdRequired")]
        public string UserId { get; set; }

        [Display(Name = "FullName")]
        [StringLength(100, ErrorMessage = "FullNameLength")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "InvalidEmailFormat")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "InvalidPhoneFormat")]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        [Display(Name = "ProfileImage")]
        public IFormFile? ProfileImage { get; set; }
    }
}
