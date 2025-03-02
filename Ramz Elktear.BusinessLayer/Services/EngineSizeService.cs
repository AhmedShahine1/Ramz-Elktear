using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.EngineSizeModels;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class EngineSizeService : IEngineSizeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public EngineSizeService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<EngineSizeDTO>> GetAllEngineSizesAsync()
        {
            var engineSizes = await _unitOfWork.EngineSizeRepository.GetAllAsync();
            return engineSizes.Select(es => _mapper.Map<EngineSizeDTO>(es));
        }

        public async Task<EngineSizeDTO> GetEngineSizeByIdAsync(string id)
        {
            var engineSize = await _unitOfWork.EngineSizeRepository.GetByIdAsync(id);
            if (engineSize == null) throw new ArgumentException("EngineSize not found");

            return _mapper.Map<EngineSizeDTO>(engineSize);
        }

        public async Task<EngineSizeDTO> AddEngineSizeAsync(AddEngineSize engineSizeDto)
        {
            var engineSize = new EngineSize
            {
                NameAr = engineSizeDto.NameAr,
                NameEn = engineSizeDto.NameEn,
                DescriptionAr = engineSizeDto.DescriptionAr,
                DescriptionEn = engineSizeDto.DescriptionEn,
                CreatedDate = DateTime.UtcNow,
                IsActive = engineSizeDto.IsActive
            };

            if (engineSizeDto.Image != null)
            {
                await SetEngineSizeImage(engineSize, engineSizeDto.Image);
            }

            await _unitOfWork.EngineSizeRepository.AddAsync(engineSize);
            await _unitOfWork.SaveChangesAsync();

            return await GetEngineSizeByIdAsync(engineSize.Id);
        }

        public async Task<EngineSizeDTO> UpdateEngineSizeAsync(UpdateEngineSize engineSizeDto)
        {
            var engineSize = await _unitOfWork.EngineSizeRepository.GetByIdAsync(engineSizeDto.Id);
            if (engineSize == null) throw new ArgumentException("EngineSize not found");

            engineSize.NameAr = engineSizeDto.NameAr;
            engineSize.NameEn = engineSizeDto.NameEn;
            engineSize.DescriptionAr = engineSizeDto.DescriptionAr;
            engineSize.DescriptionEn = engineSizeDto.DescriptionEn;
            engineSize.LastModifiedDate = DateTime.UtcNow;
            engineSize.IsActive = engineSizeDto.IsActive;

            if (engineSizeDto.Image != null)
            {
                await UpdateEngineSizeImage(engineSize, engineSizeDto.Image);
            }

            _unitOfWork.EngineSizeRepository.Update(engineSize);
            await _unitOfWork.SaveChangesAsync();

            return await GetEngineSizeByIdAsync(engineSize.Id);
        }

        public async Task<bool> DeleteEngineSizeAsync(string id)
        {
            var engineSize = await _unitOfWork.EngineSizeRepository.GetByIdAsync(id);
            if (engineSize == null) throw new ArgumentException("EngineSize not found");

            _unitOfWork.EngineSizeRepository.Delete(engineSize);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task SetEngineSizeImage(EngineSize engineSize, IFormFile image)
        {
            var path = await GetPathByName("ImageEngineSize");
            engineSize.ImageId = await _fileHandling.UploadFile(image, path);
        }

        private async Task UpdateEngineSizeImage(EngineSize engineSize, IFormFile image)
        {
            var path = await GetPathByName("ImageEngineSize");
            engineSize.ImageId = await _fileHandling.UpdateFile(image, path, engineSize.ImageId);
        }

        private async Task<string> GetEngineSizeImage(string engineSizeId)
        {
            if (string.IsNullOrEmpty(engineSizeId)) return null;
            return await _fileHandling.GetFile(engineSizeId);
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }
    }
}
