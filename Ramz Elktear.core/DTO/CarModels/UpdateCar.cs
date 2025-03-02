using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.core.DTO.CarModels
{
    public class UpdateCar
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string BrandId { get; set; }
        public string CategoryId { get; set; }
        public string ModelId { get; set; }
        public List<string> ColorIds { get; set; }
        public IEnumerable<IFormFile> Image { get; set; }
    }

}
