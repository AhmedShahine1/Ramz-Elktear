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
                var imageId = (await _unitOfWork.ImageCarRepository.FindAsync(a => a.CarId == carId && a.ColorsId == color.Id, isNoTracking: true)).ImageId;
                color.image = await _fileHandling.GetFile(imageId);
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

            return GetCarColorByCarIdAsync(carColor.CarId).Result.FirstOrDefault();
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
    }
}
