using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CategoryModels;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubCategoryDTO>> GetAllSubCategoriesAsync()
        {
            var subCategories = await _unitOfWork.SubCategoryRepository.GetAllAsync(include: q => q.Include(sc => sc.Category).Include(sc => sc.Brand));
            return subCategories.Select(sc => _mapper.Map<SubCategoryDTO>(sc));
        }

        public async Task<SubCategoryDTO> GetSubCategoryByIdAsync(string id)
        {
            var subCategory = await _unitOfWork.SubCategoryRepository.GetByIdAsync(id);
            if (subCategory == null) throw new ArgumentException("SubCategory not found");
            return _mapper.Map<SubCategoryDTO>(subCategory);
        }

        public async Task<SubCategoryDTO> AddSubCategoryAsync(CreateSubCategoryDTO subCategoryDto)
        {
            var subCategory = _mapper.Map<SubCategory>(subCategoryDto);
            await _unitOfWork.SubCategoryRepository.AddAsync(subCategory);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SubCategoryDTO>(subCategory);
        }

        public async Task<bool> UpdateSubCategoryAsync(UpdateSubCategoryDTO subCategoryDto)
        {
            var subCategory = await _unitOfWork.SubCategoryRepository.GetByIdAsync(subCategoryDto.Id);
            if (subCategory == null) throw new ArgumentException("SubCategory not found");

            _mapper.Map(subCategoryDto, subCategory);
            _unitOfWork.SubCategoryRepository.Update(subCategory);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSubCategoryAsync(string id)
        {
            var subCategory = await _unitOfWork.SubCategoryRepository.GetByIdAsync(id);
            if (subCategory == null) throw new ArgumentException("SubCategory not found");

            _unitOfWork.SubCategoryRepository.Delete(subCategory);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
