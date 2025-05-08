using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.Entities.Localization
{
    public class LocalizationValue
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ResourceId { get; set; }

        [Required]
        [StringLength(10)]
        public string CultureCode { get; set; }  // e.g., "en", "ar"

        [Required]
        public string Value { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Foreign key relationship
        [ForeignKey("ResourceId")]
        public virtual LocalizationResource Resource { get; set; }
    }
}
