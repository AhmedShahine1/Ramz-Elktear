namespace Ramz_Elktear.core.DTO.BranchModels
{
    public class BranchDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string StartWork { get; set; }
        public string EndWork { get; set; }
        public List<string> PhoneNumber { get; set; }
        public string ImageUrl { get; set; }
    }
}
