using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ramz_Elktear.core.Entities.Installment
{
    public class InstallmentRequest
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(Car))]
        public string CarId { get; set; }
        public virtual Car Car { get; set; }

        [ForeignKey(nameof(Bank))]
        public string BankId { get; set; }
        public virtual Bank Bank { get; set; }

        [ForeignKey(nameof(Job))]
        public string JobId { get; set; }
        public virtual Job Job { get; set; }

        [ForeignKey(nameof(InsurancePercentage))]
        public string InsurancePercentageId { get; set; }
        public virtual InsurancePercentage InsurancePercentage { get; set; }

        // Income & Obligations for DPR Calculation
        public decimal MonthlyIncome { get; set; }
        public decimal MonthlyObligations { get; set; }

        // Status using Enum
        public string Status { get; set; } = InstallmentStatus.Pending.ToString();

        public string gender { get; set; } = Gender.Male.ToString();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public decimal InstallmentPrice { get; private set; }

        // Number of months for the installment (e.g., 12, 15, or any user-selected value)
        public int InstallmentMonths { get; set; }

        // Function to calculate Installment Price
        public void CalculateInstallmentPrice()
        {
            decimal basePrice = Car?.SellingPrice ?? 0;
            decimal financingAmount = basePrice;

            decimal insuranceCost = ((InsurancePercentage?.Percentage ?? 0) / 100) * basePrice;
            decimal profit = 0;
            profit = (Job.Percentage / 100) * financingAmount * (InstallmentMonths / 12);
            decimal totalInstallment = profit + insuranceCost + financingAmount;

            if (InstallmentMonths > 0)
            {
                InstallmentPrice = totalInstallment / InstallmentMonths;
            }
            else
            {
                InstallmentPrice = totalInstallment;
            }
        }
    }
}
