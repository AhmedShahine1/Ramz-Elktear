using AutoMapper;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ramz_Elktear.core.Entities.Brands
{
    public class Brand
    {
        public Brand()
        {
            Cars = new HashSet<Car>();
        }

        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        [ForeignKey("Image")]
        public string ImageId { get; set; }
        public Images Image { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        [InverseProperty("Brand")]
        public virtual ICollection<Car> Cars { get; set; }
    }
}