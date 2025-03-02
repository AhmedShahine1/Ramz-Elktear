using AutoMapper;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.SpecificationModels;
using Ramz_Elktear.core.Entities.Specificate;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SpecificationDTO>> GetAllSpecificationsAsync()
        {
            var specifications = await _unitOfWork.SpecificationRepository.GetAllAsync();
            return specifications.Select(s => _mapper.Map<SpecificationDTO>(s));
        }

        public async Task<SpecificationDTO> GetSpecificationByIdAsync(string id)
        {
            var specification = await _unitOfWork.SpecificationRepository.GetByIdAsync(id);
            if (specification == null) throw new ArgumentException("Specification not found");

            return _mapper.Map<SpecificationDTO>(specification);
        }

        public async Task<SpecificationDTO> AddSpecificationAsync(AddSpecification specificationDto)
        {
            var specification = new Specification
            {
                NameAr = specificationDto.NameAr,
                NameEn = specificationDto.NameEn,
                CreatedDate = DateTime.UtcNow,
                IsActive = specificationDto.IsActive
            };

            await _unitOfWork.SpecificationRepository.AddAsync(specification);
            await _unitOfWork.SaveChangesAsync();

            return await GetSpecificationByIdAsync(specification.Id);
        }

        public async Task<SpecificationDTO> UpdateSpecificationAsync(UpdateSpecification specificationDto)
        {
            var specification = await _unitOfWork.SpecificationRepository.GetByIdAsync(specificationDto.Id);
            if (specification == null) throw new ArgumentException("Specification not found");

            specification.NameAr = specificationDto.NameAr;
            specification.NameEn = specificationDto.NameEn;
            specification.LastModifiedDate = DateTime.UtcNow;
            specification.IsActive = specificationDto.IsActive;

            _unitOfWork.SpecificationRepository.Update(specification);
            await _unitOfWork.SaveChangesAsync();

            return await GetSpecificationByIdAsync(specification.Id);
        }

        public async Task<bool> DeleteSpecificationAsync(string id)
        {
            var specification = await _unitOfWork.SpecificationRepository.GetByIdAsync(id);
            if (specification == null) throw new ArgumentException("Specification not found");

            _unitOfWork.SpecificationRepository.Delete(specification);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
