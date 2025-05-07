using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.InstallmentRequestModels
{
    public class InstallmentRequestFilter
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? CarId { get; set; }
        public string? BankId { get; set; }
        public string? JobId { get; set; }
        public Gender? Gender { get; set; }
        public InstallmentStatus? Status { get; set; }
    }
}
