namespace Ramz_Elktear.core.Entities.Specificate
{
    public class CarSpecification
    {
        public string? carId { get; set; }
        public string? SpecificationId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public Car car { get; set; }
        public Specification Specification { get; set; }
    }

}
