﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ramz_Elktear.core.DTO.RegisterModels
{
    public class RegisterSupportDeveloper
    {
        [DisplayName("Full Name")]
        [Required(ErrorMessage = "You should enter the Full Name"), StringLength(100)]
        public string FullName { get; set; }

        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "You should enter Email")]
        public string Email { get; set; }
        
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "You should enter Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You should enter the Password")]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "You should confirm the Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation do not match")]
        public string ConfirmPassword { get; set; }

        [DisplayName("Files Image")]
        public IFormFile? ImageProfile { get; set; }
    }

}
