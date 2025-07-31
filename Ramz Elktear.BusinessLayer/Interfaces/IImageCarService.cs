using Ramz_Elktear.core.DTO.ImageCarModels;
using Ramz_Elktear.core.Entities.Cars;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IImageCarService
    {
        Task<IEnumerable<string>> GetAllCarImageUrlsAsync(string carId);
        Task<IEnumerable<string>> GetAllCarImageUrlsByPathAsync(string carId, string pathName);
        Task<ImageCarDTO> GetCarImageByIdAsync(string imageCarId);
        Task<ImageCarDTO> AddCarImageAsync(AddImageCar imageCarDto);
        Task<bool> DeleteCarImageAsync(string imageCarId);
        Task<bool> DeleteAllCarImagesByPathAsync(string carId, string pathName);
        Task<IEnumerable<ImageCar>> GetAllCarImagesByPathAsync(string carId, string pathName);
    }
}
