using Ramz_Elktear.core.DTO.CarModels;
using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<CarDTO>> GetAllCarsAsync();
        Task<CarDTO> GetCarByIdAsync(string carId);
        Task<CarDTO> AddCarAsync(AddCar carDto);
        Task<CarDTO> UpdateCarAsync(UpdateCarDTO carDto);
        Task<bool> DeleteCarAsync(string carId);
        Task<IEnumerable<CarDTO>> GetCarsByBrandIdAsync(string brandId);
        Task<CarComparisonResult> CompareCarsAsync(CompareCarsRequest request);
        Task<InstallmentData> GetInstallmentAsync();
        Task<IEnumerable<CarDTO>> GetAllCarsAsync(int size = 20);
        Task<(IEnumerable<CarDTO> cars, int totalCount)> SearchCarsAsync(
                    string brandId,
                    string categoryId,
                    decimal? minPrice,
                    decimal? maxPrice,
                    int pageNumber,
                    int pageSize);
        Task<IEnumerable<CarDTO>> SearchCarsbyPageAsync(string brandId, string categoryId, string modelId, int page, int size);
        Task<IEnumerable<CarDTO>> GetCarsByOfferIdAsync(string offerId);
    }
}
