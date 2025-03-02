using Ramz_Elktear.core.DTO.FuelTypeModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IFuelTypeService
    {
        Task<IEnumerable<FuelTypeDTO>> GetAllFuelTypesAsync();
        Task<FuelTypeDTO> GetFuelTypeByIdAsync(string id);
        Task<FuelTypeDTO> AddFuelTypeAsync(AddFuelType fuelTypeDto);
        Task<FuelTypeDTO> UpdateFuelTypeAsync(UpdateFuelType fuelTypeDto);
        Task<bool> DeleteFuelTypeAsync(string id);
    }
}
