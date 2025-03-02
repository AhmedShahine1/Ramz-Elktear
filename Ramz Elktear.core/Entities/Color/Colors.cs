using Ramz_Elktear.core.Entities.Files;

namespace Ramz_Elktear.core.Entities.Color
{
    public class Colors
    {
        public Colors()
        {
            CarColors = new HashSet<CarColor>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Value { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<CarColor> CarColors { get; set; }
    }

}
