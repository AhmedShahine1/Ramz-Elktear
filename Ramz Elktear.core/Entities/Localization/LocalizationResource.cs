using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.Entities.Localization
{
    public class LocalizationResource
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ResourceKey { get; set; }

        [StringLength(100)]
        public string ResourceGroup { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<LocalizationValue> Values { get; set; }
    }
}
