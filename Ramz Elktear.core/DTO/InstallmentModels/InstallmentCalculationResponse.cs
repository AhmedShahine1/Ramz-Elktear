namespace Ramz_Elktear.core.DTO.InstallmentModels
{
    public class InstallmentCalculationResponse
    {
        public decimal MonthlyInstallment { get; set; }
        public decimal DownPayment { get; set; }
        public decimal FinalPayment { get; set; }
        public decimal InsuranceAmount { get; set; }
        public decimal TotalAmountToBePaid { get; set; }
        public bool IsApproved { get; set; }
    }
}
