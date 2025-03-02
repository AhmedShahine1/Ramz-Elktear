using Ramz_Elktear.core.DTO.CarModels;

namespace Ramz_Elktear.core.DTO.CompareModels
{
    public class CompareModel
    {
        public string ModelId { get; set; }
        public string ModelName { get; set; }
        public List<CarDTO> carDetails { get; set; }
    }
}
