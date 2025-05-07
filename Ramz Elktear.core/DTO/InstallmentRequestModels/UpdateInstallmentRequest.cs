namespace Ramz_Elktear.core.DTO.InstallmentRequestModels
{
    public class UpdateInstallmentRequest
    {
        public string Id { get; set; }
        public string CarId { get; set; }
        public string BankId { get; set; }
        public string JobId { get; set; }
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyObligations { get; set; }
        public int InstallmentMonths { get; set; }
    }
}
