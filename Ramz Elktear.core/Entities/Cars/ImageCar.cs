using Ramz_Elktear.core.Entities.Color;
using Ramz_Elktear.core.Entities.Files;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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



        [ForeignKey("Colors")]
        public string? ColorsId { get; set; }
        public virtual Colors Colors { get; set; }
    }
}
