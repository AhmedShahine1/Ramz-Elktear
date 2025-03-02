using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.OriginModels;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class OriginService : IOriginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public OriginService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<OriginDTO>> GetAllOriginsAsync()
        {
            var origins = await _unitOfWork.OriginRepository.GetAllAsync();
            return origins.Select(origin => _mapper.Map<OriginDTO>(origin));
        }

        public async Task<OriginDTO> GetOriginByIdAsync(string id)
        {
            var origin = await _unitOfWork.OriginRepository.GetByIdAsync(id);
            if (origin == null) throw new ArgumentException("Origin not found");

            return _mapper.Map<OriginDTO>(origin);
        }

        public async Task<OriginDTO> AddOriginAsync(AddOrigin originDto)
        {
            var origin = new Origin
            {
                NameAr = originDto.NameAr,
                NameEn = originDto.NameEn,
                DescriptionAr = originDto.DescriptionAr,
                DescriptionEn = originDto.DescriptionEn,
                CreatedDate = DateTime.UtcNow,
                IsActive = originDto.IsActive
            };
            await _unitOfWork.OriginRepository.AddAsync(origin);
            await _unitOfWork.SaveChangesAsync();

            return await GetOriginByIdAsync(origin.Id);
        }

        public async Task<OriginDTO> UpdateOriginAsync(UpdateOrigin originDto)
        {
            var origin = await _unitOfWork.OriginRepository.GetByIdAsync(originDto.Id);
            if (origin == null) throw new ArgumentException("Origin not found");

            origin.NameAr = originDto.NameAr;
            origin.NameEn = originDto.NameEn;
            origin.DescriptionAr = originDto.DescriptionAr;
            origin.DescriptionEn = originDto.DescriptionEn;
            origin.LastModifiedDate = DateTime.UtcNow;
            origin.IsActive = originDto.IsActive;

            _unitOfWork.OriginRepository.Update(origin);
            await _unitOfWork.SaveChangesAsync();

            return await GetOriginByIdAsync(origin.Id);
        }

        public async Task<bool> DeleteOriginAsync(string id)
        {
            var origin = await _unitOfWork.OriginRepository.GetByIdAsync(id);
            if (origin == null) throw new ArgumentException("Origin not found");

            _unitOfWork.OriginRepository.Delete(origin);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
