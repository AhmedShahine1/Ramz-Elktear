using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.ImageCarModels;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class ImageCarService : IImageCarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;

        public ImageCarService(IUnitOfWork unitOfWork, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<string>> GetAllCarImageUrlsAsync(string carId)
        {
            var images = await _unitOfWork.ImageCarRepository.FindAllAsync(x => x.CarId == carId);
            var imageUrls = new List<string>();

            foreach (var image in images)
            {
                var imageUrl = await _fileHandling.GetFile(image.ImageId);
                if (!string.IsNullOrEmpty(imageUrl))  // Ensure the URL is not null or empty
                {
                    imageUrls.Add(imageUrl);
                }
            }

            return imageUrls;
        }

        public async Task<IEnumerable<string>> GetAllCarImageUrlsByPathAsync(string carId, string pathName)
        {
            // Fetch images related to the car and filter by path name
            var images = await _unitOfWork.ImageCarRepository.FindAllAsync(x =>
                x.CarId == carId && x.Image.path.Name == pathName,
                include: q=>q.Include(a=>a.Image).ThenInclude(a=>a.path));

            var imageUrls = new List<string>();

            foreach (var image in images)
            {
                var imageUrl = await _fileHandling.GetFile(image.ImageId);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    imageUrls.Add(imageUrl);
                }
            }

            return imageUrls;
        }

        public async Task<ImageCarDTO> GetCarImageByIdAsync(string imageCarId)
        {
            var imageCar = await _unitOfWork.ImageCarRepository.GetByIdAsync(imageCarId);
            if (imageCar == null) throw new ArgumentException("Car image not found");

            return new ImageCarDTO()
            {
                CarId = imageCar.CarId,
                ImageId = imageCar.ImageId,
                ImageUrl = await _fileHandling.GetFile(imageCar.ImageId)
            };
        }

        public async Task<ImageCarDTO> AddCarImageAsync(AddImageCar imageCarDto)
        {
            var imageId = await _fileHandling.UploadFile(imageCarDto.Image, imageCarDto.paths);

            var imageCar = new ImageCar()
            {
                CarId = imageCarDto.CarId,
                ImageId = imageId
            };

            await _unitOfWork.ImageCarRepository.AddAsync(imageCar);
            await _unitOfWork.SaveChangesAsync();

            return new ImageCarDTO()
            {
                CarId = imageCar.CarId,
                ImageId = imageCar.ImageId,
                ImageUrl = await _fileHandling.GetFile(imageId)
            };
        }

        public async Task<bool> DeleteCarImageAsync(string imageCarId)
        {
            var imageCar = await _unitOfWork.ImageCarRepository.GetByIdAsync(imageCarId);
            if (imageCar == null) throw new ArgumentException("Car image not found");

            await _fileHandling.DeleteFile(imageCar.ImageId);
            _unitOfWork.ImageCarRepository.Delete(imageCar);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the car image: " + ex.Message, ex);
            }
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }
    }
}
