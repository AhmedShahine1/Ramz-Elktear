using Ramz_Elktear.core.Entities.Files;

namespace Ramz_Elktear.core.Entities.Cars
{
    public class ImageCar
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CarId { get; set; }
        public Car Car { get; set; }
        public string ImageId { get; set; }
    }
}
