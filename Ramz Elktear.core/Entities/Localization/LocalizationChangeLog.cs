using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.Entities.Localization
{
    public class LocalizationChangeLog
    {
        [Key]
        public int Id { get; set; }

        public int ResourceId { get; set; }

        [StringLength(10)]
        public string CultureCode { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("ResourceId")]
        public virtual LocalizationResource Resource { get; set; }
    }
}
