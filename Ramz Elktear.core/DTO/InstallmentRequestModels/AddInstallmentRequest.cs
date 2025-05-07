using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.InstallmentRequestModels
{
    public class AddInstallmentRequest
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string CarId { get; set; }
        public required string Age { get; set; }
        public required string BankId { get; set; }
        public required string JobId { get; set; }
        public required decimal MonthlyIncome { get; set; }
        public required Gender Gender { get; set; }
        public required decimal MonthlyObligations { get; set; }
        public required int InstallmentMonths { get; set; }
    }
}
