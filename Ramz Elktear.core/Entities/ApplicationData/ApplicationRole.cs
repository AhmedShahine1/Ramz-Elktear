using Microsoft.AspNetCore.Identity;

namespace Ramz_Elktear.core.Entities.ApplicationData
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
        public string ArName { get; set; }
    }
}
