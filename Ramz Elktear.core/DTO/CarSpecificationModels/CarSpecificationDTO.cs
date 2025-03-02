namespace Ramz_Elktear.core.DTO.CarSpecificationModels
{
    public class CarSpecificationDTO
    {
        public string CarId { get; set; }
        public string SpecificationId { get; set; }
        public string CarModel { get; set; }  // Name/Model of the car
        public string SpecificationName { get; set; }  // Name of the specification
        public bool IsActive { get; set; }
    }
}
