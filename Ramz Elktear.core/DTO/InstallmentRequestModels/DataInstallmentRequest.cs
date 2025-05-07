using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.InstallmentModels;

namespace Ramz_Elktear.core.DTO.InstallmentRequestModels
{
    public class DataInstallmentRequest
    {
        public IEnumerable<CarDTO> Car { get; set; }
        public IEnumerable<JobDTO> Job { get; set; }
        public IEnumerable<BankDetails> Bank { get; set; }
    }
}
