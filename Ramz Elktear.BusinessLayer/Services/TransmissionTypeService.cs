using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.TransmissionTypeModels;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class TransmissionTypeService : ITransmissionTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public TransmissionTypeService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<TransmissionTypeDTO>> GetAllTransmissionTypesAsync()
        {
            var transmissionTypes = await _unitOfWork.TransmissionTypeRepository.GetAllAsync();
            return transmissionTypes.Select(t => _mapper.Map<TransmissionTypeDTO>(t));
        }

        public async Task<TransmissionTypeDTO> GetTransmissionTypeByIdAsync(string id)
        {
            var transmissionType = await _unitOfWork.TransmissionTypeRepository.GetByIdAsync(id);
            if (transmissionType == null) throw new ArgumentException("TransmissionType not found");

            return _mapper.Map<TransmissionTypeDTO>(transmissionType);
        }

        public async Task<TransmissionTypeDTO> AddTransmissionTypeAsync(AddTransmissionType transmissionTypeDto)
        {
            var transmissionType = new TransmissionType
            {
                NameAr = transmissionTypeDto.NameAr,
                NameEn = transmissionTypeDto.NameEn,
                DescriptionAr = transmissionTypeDto.DescriptionAr,
                DescriptionEn = transmissionTypeDto.DescriptionEn,
                CreatedDate = DateTime.UtcNow,
                IsActive = transmissionTypeDto.IsActive
            };

            if (transmissionTypeDto.Image != null)
            {
                await SetTransmissionTypeImage(transmissionType, transmissionTypeDto.Image);
            }

            await _unitOfWork.TransmissionTypeRepository.AddAsync(transmissionType);
            await _unitOfWork.SaveChangesAsync();

            return await GetTransmissionTypeByIdAsync(transmissionType.Id);
        }

        public async Task<TransmissionTypeDTO> UpdateTransmissionTypeAsync(UpdateTransmissionType transmissionTypeDto)
        {
            var transmissionType = await _unitOfWork.TransmissionTypeRepository.GetByIdAsync(transmissionTypeDto.Id);
            if (transmissionType == null) throw new ArgumentException("TransmissionType not found");

            transmissionType.NameAr = transmissionTypeDto.NameAr;
            transmissionType.NameEn = transmissionTypeDto.NameEn;
            transmissionType.DescriptionAr = transmissionTypeDto.DescriptionAr;
            transmissionType.DescriptionEn = transmissionTypeDto.DescriptionEn;
            transmissionType.LastModifiedDate = DateTime.UtcNow;
            transmissionType.IsActive = transmissionTypeDto.IsActive;

            if (transmissionTypeDto.Image != null)
            {
                await UpdateTransmissionTypeImage(transmissionType, transmissionTypeDto.Image);
            }

            _unitOfWork.TransmissionTypeRepository.Update(transmissionType);
            await _unitOfWork.SaveChangesAsync();

            return await GetTransmissionTypeByIdAsync(transmissionType.Id);
        }

        public async Task<bool> DeleteTransmissionTypeAsync(string id)
        {
            var transmissionType = await _unitOfWork.TransmissionTypeRepository.GetByIdAsync(id);
            if (transmissionType == null) throw new ArgumentException("TransmissionType not found");

            _unitOfWork.TransmissionTypeRepository.Delete(transmissionType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task SetTransmissionTypeImage(TransmissionType transmissionType, IFormFile image)
        {
            var path = await GetPathByName("ImageTransmissionType");
            transmissionType.ImageId = await _fileHandling.UploadFile(image, path);
        }

        private async Task UpdateTransmissionTypeImage(TransmissionType transmissionType, IFormFile image)
        {
            var path = await GetPathByName("ImageTransmissionType");
            transmissionType.ImageId = await _fileHandling.UpdateFile(image, path, transmissionType.ImageId);
        }

        private async Task<string> GetTransmissionTypeImage(string transmissionTypeId)
        {
            if (string.IsNullOrEmpty(transmissionTypeId)) return null;
            return await _fileHandling.GetFile(transmissionTypeId);
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }
    }
}
