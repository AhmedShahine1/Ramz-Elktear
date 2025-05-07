namespace Ramz_Elktear.core.DTO.AuthModels
{
    public class UserRoleDTO
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
