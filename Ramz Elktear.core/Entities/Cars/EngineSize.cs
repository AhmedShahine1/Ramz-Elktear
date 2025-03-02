using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ramz_Elktear.core.Entities.Cars
{
    public class EngineSize
    {
        public EngineSize()
        {
            Cars = new HashSet<Car>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string? ImageId { get; set; }
        public Images? Image { get; set; }

        [InverseProperty("EngineSize")]
        public virtual ICollection<Car> Cars { get; set; }
    }

}
