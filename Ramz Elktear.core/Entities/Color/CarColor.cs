using Ramz_Elktear.core.Entities.Cars;
using System.Drawing;

namespace Ramz_Elktear.core.Entities.Color
{
    public class CarColor
    {
        public string CarId { get; set; }
        public string? ColorId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Colors Color { get; set; }
        public Car Car { get; set; }
    }

}
