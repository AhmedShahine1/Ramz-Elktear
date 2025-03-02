using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ramz_Elktear.core.DTO.RegisterModels
{
    public class RegisterCustomer
    {
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "You should enter the Phone Number"), StringLength(15)]
        public string PhoneNumber { get; set; }

        [DisplayName("Profile Image")]
        public IFormFile? ImageProfile { get; set; }
    }
}
