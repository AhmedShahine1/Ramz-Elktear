using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.BranchModels
{
    public class AddBranch
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string StartWork { get; set; }
        public string EndWork { get; set; }
        public List<string>? PhoneNumber { get; set; }
        public IFormFile Image { get; set; }
        public string Latitude { get; set; }  // Add Latitude
        public string Longitude { get; set; } // Add Longitude
    }

}
