namespace Ramz_Elktear.core.Entities.Branchs
{
    public class Branch
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string StartWork { get; set; }
        public string EndWork { get; set; }
        public string ImageId { get; set; }
        public List<string> PhoneNumber { get; set; }
    }
}
