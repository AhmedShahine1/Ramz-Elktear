using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.core.DTO.InstallmentModels
{
    public class JobDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public bool IsConvertable { get; set; }
    }
}
