using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.core.Entities.Cars
{
    public class ImageCar
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey("Car")]
        public string CarId { get; set; }
        public virtual Car Car { get; set; }

        [ForeignKey("Image")]
        public string ImageId { get; set; }
        public virtual Images Image { get; set; }
    }
}
