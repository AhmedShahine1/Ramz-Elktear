using Ramz_Elktear.core.DTO.ImageCarModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IImageCarService
    {
        Task<IEnumerable<string>> GetAllCarImageUrlsAsync(string carId);
        Task<IEnumerable<string>> GetAllCarImageUrlsByPathAsync(string carId, string pathName);
        Task<ImageCarDTO> GetCarImageByIdAsync(string imageCarId);
        Task<ImageCarDTO> AddCarImageAsync(AddImageCar imageCarDto);
        Task<bool> DeleteCarImageAsync(string imageCarId);
    }
}
