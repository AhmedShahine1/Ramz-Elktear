using Ramz_Elktear.core.DTO.InsurancePercentageModels;
using Ramz_Elktear.core.Entities.Installment;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IInsurancePercentageService
    {
        Task<IEnumerable<InsurancePercentageDTO>> GetAllAsync();
        Task<InsurancePercentageDTO> GetByIdAsync(string id);
        Task<bool> AddAsync(AddInsurancePercentageDTO dto);
        Task<bool> UpdateAsync(InsurancePercentageDTO dto);
        Task<bool> DeleteAsync(string id);
    }
}
