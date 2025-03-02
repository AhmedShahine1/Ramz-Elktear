using Ramz_Elktear.core.DTO.OriginModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IOriginService
    {
        Task<IEnumerable<OriginDTO>> GetAllOriginsAsync();
        Task<OriginDTO> GetOriginByIdAsync(string id);
        Task<OriginDTO> AddOriginAsync(AddOrigin originDto);
        Task<OriginDTO> UpdateOriginAsync(UpdateOrigin originDto);
        Task<bool> DeleteOriginAsync(string id);
    }
}
