using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.InstallmentRequestModels
{
    public class DetailsInstallmentRequest
    {
        public string Id { get; set; }
        public AuthDTO User { get; set; }
        public BankDetails Bank {  get; set; }
        public JobDTO Job { get; set; }
        public CarDTO Car { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyObligations { get; set; }
        public decimal InstallmentPrice { get; set; }
        public InstallmentStatus Status { get; set; }
        public Gender Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public int InstallmentMonths { get; set; }
    }
}
