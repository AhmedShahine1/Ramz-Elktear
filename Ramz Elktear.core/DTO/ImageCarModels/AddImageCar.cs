using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.Entities.Files;

namespace Ramz_Elktear.core.DTO.ImageCarModels
{
    public class AddImageCar
    {
        public string CarId { get; set; }
        public IFormFile Image { get; set; }
        public Paths paths { get; set; }
    }
}