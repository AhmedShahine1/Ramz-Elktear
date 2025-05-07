using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.InsurancePercentageModels
{
    public class AddInsurancePercentageDTO
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public Gender Gender { get; set; }
        public decimal Percentage { get; set; }
    }
}
