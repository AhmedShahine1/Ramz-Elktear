using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.FuelTypeModels;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class FuelTypeService : IFuelTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public FuelTypeService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }
        public async Task<IEnumerable<FuelTypeDTO>> GetAllFuelTypesAsync()
        {
            var fuelTypes = await _unitOfWork.FuelTypeRepository.GetAllAsync();
            var fuelTypeDTOs = fuelTypes.Select(f => _mapper.Map<FuelTypeDTO>(f)).ToList();

            foreach (var dto in fuelTypeDTOs)
            {
                dto.ImageUrl = await GetFuelTypeImage(fuelTypes.Where(q => q.Id == dto.Id).Select(q => q.ImageId).FirstOrDefault());
            }

            return fuelTypeDTOs;
        }

        public async Task<FuelTypeDTO> GetFuelTypeByIdAsync(string id)
        {
            var fuelType = await _unitOfWork.FuelTypeRepository.GetByIdAsync(id);
            if (fuelType == null) throw new ArgumentException("FuelType not found");

            var fuelTypeDTO = _mapper.Map<FuelTypeDTO>(fuelType);
            fuelTypeDTO.ImageUrl = await GetFuelTypeImage(fuelType.ImageId);

            return fuelTypeDTO;
        }

        public async Task<FuelTypeDTO> AddFuelTypeAsync(AddFuelType fuelTypeDto)
        {
            var fuelType = new FuelType
            {
                NameAr = fuelTypeDto.NameAr,
                NameEn = fuelTypeDto.NameEn,
                DescriptionAr = fuelTypeDto.DescriptionAr,
                DescriptionEn = fuelTypeDto.DescriptionEn,
                CreatedDate = DateTime.UtcNow,
                IsActive = fuelTypeDto.IsActive
            };

            if (fuelTypeDto.Image != null)
            {
                await SetFuelTypeImage(fuelType, fuelTypeDto.Image);
            }

            await _unitOfWork.FuelTypeRepository.AddAsync(fuelType);
            await _unitOfWork.SaveChangesAsync();

            var addedFuelType = await GetFuelTypeByIdAsync(fuelType.Id);
            addedFuelType.ImageUrl = await GetFuelTypeImage(fuelType.ImageId);

            return addedFuelType;
        }

        public async Task<FuelTypeDTO> UpdateFuelTypeAsync(UpdateFuelType fuelTypeDto)
        {
            var fuelType = await _unitOfWork.FuelTypeRepository.GetByIdAsync(fuelTypeDto.Id);
            if (fuelType == null) throw new ArgumentException("FuelType not found");

            fuelType.NameAr = fuelTypeDto.NameAr;
            fuelType.NameEn = fuelTypeDto.NameEn;
            fuelType.DescriptionAr = fuelTypeDto.DescriptionAr;
            fuelType.DescriptionEn = fuelTypeDto.DescriptionEn;
            fuelType.LastModifiedDate = DateTime.UtcNow;
            fuelType.IsActive = fuelTypeDto.IsActive;

            if (fuelTypeDto.Image != null)
            {
                await UpdateFuelTypeImage(fuelType, fuelTypeDto.Image);
            }

            _unitOfWork.FuelTypeRepository.Update(fuelType);
            await _unitOfWork.SaveChangesAsync();

            var updatedFuelType = await GetFuelTypeByIdAsync(fuelType.Id);
            updatedFuelType.ImageUrl = await GetFuelTypeImage(fuelType.ImageId);

            return updatedFuelType;
        }

        public async Task<bool> DeleteFuelTypeAsync(string id)
        {
            var fuelType = await _unitOfWork.FuelTypeRepository.GetByIdAsync(id);
            if (fuelType == null) throw new ArgumentException("FuelType not found");

            _unitOfWork.FuelTypeRepository.Delete(fuelType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task SetFuelTypeImage(FuelType fuelType, IFormFile image)
        {
            var path = await GetPathByName("ImageFuelType");
            fuelType.ImageId = await _fileHandling.UploadFile(image, path);
        }

        private async Task UpdateFuelTypeImage(FuelType fuelType, IFormFile image)
        {
            var path = await GetPathByName("ImageFuelType");
            fuelType.ImageId = await _fileHandling.UpdateFile(image, path, fuelType.ImageId);
        }

        private async Task<string> GetFuelTypeImage(string fuelTypeId)
        {
            if (string.IsNullOrEmpty(fuelTypeId)) return null;
            return await _fileHandling.GetFile(fuelTypeId);
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }
    }
}
