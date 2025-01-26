using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.Entities.Cars;

namespace Ramz_Elktear.core.DTO.BrandModels
{
    public class BrandWithDetails
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public IEnumerable<CarDetails> Cars { get; set; }
        public IEnumerable<CarModel> Models { get; set; }
        public IEnumerable<CarCategory> Categories { get; set; }
    }
}
