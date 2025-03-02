using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ramz_Elktear.core.DTO.AuthModels
{
    public class AuthDTO
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
    }
}
