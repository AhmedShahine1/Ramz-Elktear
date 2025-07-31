using AutoMapper;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CarColorModels;
using Ramz_Elktear.core.Entities.Color;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using Ramz_Elktear.core.DTO.ColorModels;
using Microsoft.EntityFrameworkCore;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class CarColorService : ICarColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public CarColorService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<CarColorDTO>> GetAllCarColorsAsync()
        {
            var carColors = await _unitOfWork.CarColorRepository.GetAllAsync();
            return carColors.Select(cc => _mapper.Map<CarColorDTO>(cc));
        }

        public async Task<IEnumerable<ColorDTO>> GetCarColorByCarIdAsync(string carId)
        {
            var carColor = await _unitOfWork.CarColorRepository.FindAllAsync(q => q.CarId == carId, include: a => a.Include(i => i.Color));
            var CarColorDTO = carColor.Select(carcolor => _mapper.Map<ColorDTO>(carcolor.Color)).ToList();
            foreach (var color in CarColorDTO)
            {
                var imageEntities = await _unitOfWork.ImageCarRepository.FindAllAsync(
                    a => a.CarId == carId && a.ColorsId == color.Id,
                    isNoTracking: true);

                color.images ??= new List<string>();

                color.images.Clear();

                foreach (var imageEntity in imageEntities)
                {
                    var imageData = await _fileHandling.GetFile(imageEntity.ImageId);
                    if (!string.IsNullOrEmpty(imageData))
                    {
                        color.images.Add(imageData);
                    }
                }
            }
            return CarColorDTO;
        }

        public async Task<ColorDTO> AddCarColorAsync(AddCarColor carColorDto)
        {
            var carColor = new CarColor
            {
                CarId = carColorDto.CarId,
                ColorId = carColorDto.ColorId,
                CreatedDate = DateTime.UtcNow,
                IsActive = carColorDto.IsActive
            };

            await _unitOfWork.CarColorRepository.AddAsync(carColor);
            await _unitOfWork.SaveChangesAsync();

            // Get the color details without the image
            var color = await _unitOfWork.ColorsRepository.FindAsync(c => c.Id == carColorDto.ColorId);
            if (color == null)
            {
                throw new Exception("Color not found");
            }

            var colorDto = _mapper.Map<ColorDTO>(color);
            colorDto.images = null;

            return colorDto;
        }

        public async Task<ColorDTO> UpdateCarColorAsync(string carId, AddCarColor carColorDto)
        {
            var carColor = await _unitOfWork.CarColorRepository.FindAsync(q => q.CarId == carId);
            if (carColor == null) throw new ArgumentException("CarColor not found for the given carId");

            carColor.ColorId = carColorDto.ColorId;
            carColor.LastModifiedDate = DateTime.UtcNow;
            carColor.IsActive = carColorDto.IsActive;

            _unitOfWork.CarColorRepository.Update(carColor);
            await _unitOfWork.SaveChangesAsync();

            return GetCarColorByCarIdAsync(carColor.CarId).Result.FirstOrDefault();
        }

        public async Task<bool> DeleteCarColorAsync(string carId , string colorId)
        {
            var carColor = await _unitOfWork.CarColorRepository.FindAsync(q => q.CarId == carId && q.ColorId == colorId);
            if (carColor == null) throw new ArgumentException("CarColor not found for the given carId");

            _unitOfWork.CarColorRepository.Delete(carColor);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAllCarColorsAsync(string carId)
        {
            var carColors = await _unitOfWork.CarColorRepository.FindAllAsync(q => q.CarId == carId);
            if (carColors != null && carColors.Any())
            {
                _unitOfWork.CarColorRepository.DeleteRange(carColors);
                await _unitOfWork.SaveChangesAsync();
            }
            return true;
        }
    }
}
