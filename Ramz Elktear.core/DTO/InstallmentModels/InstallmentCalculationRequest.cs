using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.InstallmentModels
{
    public class InstallmentCalculationRequest
    {
        public decimal CarPrice { get; set; }
        public decimal Salary { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string JobSector { get; set; }
        public string BankId { get; set; }
        public string InstallmentPlanId { get; set; }
    }
}
