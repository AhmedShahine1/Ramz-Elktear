using Ramz_Elktear.core.DTO.EnginePositionModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IEnginePositionService
    {
        Task<IEnumerable<EnginePositionDTO>> GetAllEnginePositionsAsync();
        Task<EnginePositionDTO> GetEnginePositionByIdAsync(string id);
        Task<EnginePositionDTO> AddEnginePositionAsync(AddEnginePosition enginePositionDto);
        Task<EnginePositionDTO> UpdateEnginePositionAsync(UpdateEnginePosition enginePositionDto);
        Task<bool> DeleteEnginePositionAsync(string id);
    }
}
