using Ramz_Elktear.core.DTO.ModelYearModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IModelYearService
    {
        Task<IEnumerable<ModelYearDTO>> GetAllModelYearsAsync();
        Task<ModelYearDTO> GetModelYearByIdAsync(string id);
        Task<ModelYearDTO> AddModelYearAsync(AddModelYear modelYearDto);
        Task<ModelYearDTO> UpdateModelYearAsync(UpdateModelYear modelYearDto);
        Task<bool> DeleteModelYearAsync(string id);
    }
}
