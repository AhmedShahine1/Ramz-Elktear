using Ramz_Elktear.core.DTO.SpecificationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ISpecificationService
    {
        Task<IEnumerable<SpecificationDTO>> GetAllSpecificationsAsync();
        Task<SpecificationDTO> GetSpecificationByIdAsync(string id);
        Task<SpecificationDTO> AddSpecificationAsync(AddSpecification specificationDto);
        Task<SpecificationDTO> UpdateSpecificationAsync(UpdateSpecification specificationDto);
        Task<bool> DeleteSpecificationAsync(string id);
    }
}
