using Ramz_Elktear.core.DTO.EngineSizeModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IEngineSizeService
    {
        Task<IEnumerable<EngineSizeDTO>> GetAllEngineSizesAsync();
        Task<EngineSizeDTO> GetEngineSizeByIdAsync(string id);
        Task<EngineSizeDTO> AddEngineSizeAsync(AddEngineSize engineSizeDto);
        Task<EngineSizeDTO> UpdateEngineSizeAsync(UpdateEngineSize engineSizeDto);
        Task<bool> DeleteEngineSizeAsync(string id);
    }
}
