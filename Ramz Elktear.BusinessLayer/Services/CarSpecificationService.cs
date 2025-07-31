using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CarSpecificationModels;
using Ramz_Elktear.core.DTO.SpecificationModels;
using Ramz_Elktear.core.Entities.Specificate;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class CarSpecificationService : ICarSpecificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarSpecificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SpecificationDTO>> GetSpecificationsByCarIdAsync(string carId)
        {
            var carSpecifications = await _unitOfWork.CarSpecificationRepository.FindAllAsync(q => q.carId == carId, include: a => a.Include(i => i.Specification));
            if (carSpecifications == null || !carSpecifications.Any())
                return Enumerable.Empty<SpecificationDTO>();

            return carSpecifications.Select(carSpecification => _mapper.Map<SpecificationDTO>(carSpecification.Specification));
        }

        public async Task<IEnumerable<CarSpecificationDTO>> GetCarsBySpecificationIdAsync(string specificationId)
        {
            var carSpecifications = await _unitOfWork.CarSpecificationRepository.FindAllAsync(q => q.SpecificationId == specificationId);
            if (carSpecifications == null || !carSpecifications.Any())
                throw new ArgumentException("No cars found for the given specificationId");

            return carSpecifications.Select(carSpecification => _mapper.Map<CarSpecificationDTO>(carSpecification));
        }

        public async Task<CarSpecificationDTO> AddCarSpecificationAsync(AddCarSpecification carSpecificationDto)
        {
            var carSpecification = new CarSpecification
            {
                carId = carSpecificationDto.CarId,
                SpecificationId = carSpecificationDto.SpecificationId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            await _unitOfWork.CarSpecificationRepository.AddAsync(carSpecification);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CarSpecificationDTO>(carSpecification);
        }

        public async Task<bool> DeleteCarSpecificationAsync(string carId, string specificationId)
        {
            var carSpecification = await _unitOfWork.CarSpecificationRepository.FindAllAsync(q => q.carId == carId && q.SpecificationId == specificationId);
            if (carSpecification == null) throw new ArgumentException("CarSpecification not found for the given carId and specificationId");

            _unitOfWork.CarSpecificationRepository.DeleteRange(carSpecification);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAllCarSpecificationsAsync(string carId)
        {
            var carSpecifications = await _unitOfWork.CarSpecificationRepository.FindAllAsync(q => q.carId == carId);
            if (carSpecifications != null && carSpecifications.Any())
            {
                _unitOfWork.CarSpecificationRepository.DeleteRange(carSpecifications);
                await _unitOfWork.SaveChangesAsync();
            }
            return true;
        }
    }
}
