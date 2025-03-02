using Ramz_Elktear.core.DTO.CarColorModels;
using Ramz_Elktear.core.DTO.ColorModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ICarColorService
    {
        Task<IEnumerable<CarColorDTO>> GetAllCarColorsAsync();
        Task<IEnumerable<ColorDTO>> GetCarColorByCarIdAsync(string carId);
        Task<ColorDTO> AddCarColorAsync(AddCarColor carColorDto);
        Task<ColorDTO> UpdateCarColorAsync(string carId, AddCarColor carColorDto);
        Task<bool> DeleteCarColorAsync(string carId , string colorId);
    }
}
