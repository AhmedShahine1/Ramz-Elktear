using Ramz_Elktear.core.DTO.AuthModels;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.DTO.NotificationModels
{
    public class SendNotificationViewModel
    {
        [Required]
        public List<string> UserIds { get; set; } = new List<string>(); // Ensure it's initialized

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty; // Ensure default value

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty; // Ensure default value
    }
}
