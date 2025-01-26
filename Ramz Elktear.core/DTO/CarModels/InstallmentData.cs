using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.CityModels;
using Ramz_Elktear.core.DTO.CompareModels;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.Entities.Cars;

namespace Ramz_Elktear.core.DTO.CarModels
{
    public class InstallmentData
    {
        public IEnumerable<BankDetails> Banks { get; set; }
        public IEnumerable<JobDetails> Jobs { get; set; }
        public IEnumerable<CompareBrand> BrandDetails { get; set; }
        public IEnumerable<CityDto> Cities { get; set; }
    }
}
