using Ramz_Elktear.core.DTO.CityModels;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public interface ICityService
    {
        Task<IEnumerable<CityDto>> GetAllCitiesAsync();
        Task<CityDto> GetCityByIdAsync(string cityId);
        Task<CityDto> AddCityAsync(AddCityDto addCityDto);
        Task<bool> UpdateCityAsync(UpdateCityDto updateCityDto);
        Task<bool> DeleteCityAsync(string cityId);
    }
}
