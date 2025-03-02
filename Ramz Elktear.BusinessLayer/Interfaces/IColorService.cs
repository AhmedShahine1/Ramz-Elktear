using Ramz_Elktear.core.DTO.ColorModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IColorService
    {
        Task<IEnumerable<ColorDTO>> GetAllColorsAsync();
        Task<ColorDTO> GetColorByIdAsync(string id);
        Task<ColorDTO> AddColorAsync(AddColor colorDto);
        Task<ColorDTO> UpdateColorAsync(UpdateColor colorDto);
        Task<bool> DeleteColorAsync(string id);
    }
}
