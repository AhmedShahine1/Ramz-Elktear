using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.EnginePositionModels;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class EnginePositionService : IEnginePositionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public EnginePositionService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<EnginePositionDTO>> GetAllEnginePositionsAsync()
        {
            var enginePositions = await _unitOfWork.EnginePositionRepository.GetAllAsync();
            return enginePositions.Select(ep => _mapper.Map<EnginePositionDTO>(ep));
        }

        public async Task<EnginePositionDTO> GetEnginePositionByIdAsync(string id)
        {
            var enginePosition = await _unitOfWork.EnginePositionRepository.GetByIdAsync(id);
            if (enginePosition == null) throw new ArgumentException("EnginePosition not found");

            return _mapper.Map<EnginePositionDTO>(enginePosition);
        }

        public async Task<EnginePositionDTO> AddEnginePositionAsync(AddEnginePosition enginePositionDto)
        {
            var enginePosition = new EnginePosition
            {
                NameAr = enginePositionDto.NameAr,
                NameEn = enginePositionDto.NameEn,
                DescriptionAr = enginePositionDto.DescriptionAr,
                DescriptionEn = enginePositionDto.DescriptionEn,
                CreatedDate = DateTime.UtcNow,
                IsActive = enginePositionDto.IsActive
            };

            if (enginePositionDto.Image != null)
            {
                await SetEnginePositionImage(enginePosition, enginePositionDto.Image);
            }

            await _unitOfWork.EnginePositionRepository.AddAsync(enginePosition);
            await _unitOfWork.SaveChangesAsync();

            return await GetEnginePositionByIdAsync(enginePosition.Id);
        }

        public async Task<EnginePositionDTO> UpdateEnginePositionAsync(UpdateEnginePosition enginePositionDto)
        {
            var enginePosition = await _unitOfWork.EnginePositionRepository.GetByIdAsync(enginePositionDto.Id);
            if (enginePosition == null) throw new ArgumentException("EnginePosition not found");

            enginePosition.NameAr = enginePositionDto.NameAr;
            enginePosition.NameEn = enginePositionDto.NameEn;
            enginePosition.DescriptionAr = enginePositionDto.DescriptionAr;
            enginePosition.DescriptionEn = enginePositionDto.DescriptionEn;
            enginePosition.LastModifiedDate = DateTime.UtcNow;
            enginePosition.IsActive = enginePositionDto.IsActive;

            if (enginePositionDto.Image != null)
            {
                await UpdateEnginePositionImage(enginePosition, enginePositionDto.Image);
            }

            _unitOfWork.EnginePositionRepository.Update(enginePosition);
            await _unitOfWork.SaveChangesAsync();

            return await GetEnginePositionByIdAsync(enginePosition.Id);
        }

        public async Task<bool> DeleteEnginePositionAsync(string id)
        {
            var enginePosition = await _unitOfWork.EnginePositionRepository.GetByIdAsync(id);
            if (enginePosition == null) throw new ArgumentException("EnginePosition not found");

            _unitOfWork.EnginePositionRepository.Delete(enginePosition);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task SetEnginePositionImage(EnginePosition enginePosition, IFormFile image)
        {
            var path = await GetPathByName("ImageEnginePosition");
            enginePosition.ImageId = await _fileHandling.UploadFile(image, path);
        }

        private async Task UpdateEnginePositionImage(EnginePosition enginePosition, IFormFile image)
        {
            var path = await GetPathByName("ImageEnginePosition");
            enginePosition.ImageId = await _fileHandling.UpdateFile(image, path, enginePosition.ImageId);
        }

        private async Task<string> GetEnginePositionImage(string enginePositionId)
        {
            if (string.IsNullOrEmpty(enginePositionId)) return null;
            return await _fileHandling.GetFile(enginePositionId);
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }
    }
}
