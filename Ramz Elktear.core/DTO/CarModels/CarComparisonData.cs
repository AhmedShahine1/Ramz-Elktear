using Ramz_Elktear.core.Entities.Cars;

namespace Ramz_Elktear.core.DTO.CarModels
{
    public class CarComparisonData
    {
        public IEnumerable<CarDetails> Cars { get; set; }
        public IEnumerable<CarModel> Models { get; set; }
        public IEnumerable<CarCategory> Categories { get; set; }
        public IEnumerable<string> Types { get; set; }
    }
}
