using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.Entities.Cars;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarDetails>> GetAllCarsAsync();
        Task<CarDetails> GetCarByIdAsync(string carId);
        Task<CarDetails> AddCarAsync(AddCar carDto);
        //Task<bool> UpdateCarAsync(string carId, CarDetails carDto);
        //Task<bool> DeleteCarAsync(string carId);
        Task<bool> AddCarColorAsync(CarColor color);
        Task<bool> AddCarCategoryAsync(CarCategory category);
        Task<bool> AddCarModelAsync(CarModel model);
        Task<IEnumerable<CarColor>> GetAllCarColorsAsync();
        Task<IEnumerable<CarCategory>> GetAllCarCategoriesAsync();
        Task<IEnumerable<CarModel>> GetAllCarModelsAsync();
        Task<CarComparisonResult> CompareCarsAsync(CompareCarsRequest request);
        Task<CarComparisonData> GetCarComparisonDataAsync();
        Task<InstallmentData> GetInstallmentAsync();
        Task<IEnumerable<CarDetails>> GetCarsByBrandAsync(string brandId);
        Task<IEnumerable<BrandWithDetails>> GetCarComparisonDataWithBrandAsync();
    }
}
