using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ramz_Elktear.core.Entities.Promotion
{
    public class Promotion
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [ForeignKey(nameof(ImageAr))]
        public string ImageArId { get; set; }
        public Images ImageAr { get; set; }
        [ForeignKey(nameof(ImageEn))]
        public string ImageEnId { get; set; }
        public Images ImageEn { get; set; } 
        public string redirctURL { get; set; }
        public bool IsActive { get; set; }
    }
}
