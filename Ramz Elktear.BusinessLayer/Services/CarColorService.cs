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
        private readonly IMapper _mapper;

        public CarColorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CarColorDTO>> GetAllCarColorsAsync()
        {
            var carColors = await _unitOfWork.CarColorRepository.GetAllAsync();
            return carColors.Select(cc => _mapper.Map<CarColorDTO>(cc));
        }

        public async Task<IEnumerable<ColorDTO>> GetCarColorByCarIdAsync(string carId)
        {
            var carColor = await _unitOfWork.CarColorRepository.FindAllAsync(q => q.CarId == carId, include: a => a.Include(i => i.Color));
            return carColor.Select(carcolor => _mapper.Map<ColorDTO>(carcolor.Color));
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
