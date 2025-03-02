using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CategoryModels;
using Ramz_Elktear.core.Entities.Categories;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using Ramz_Elktear.core.Entities.Files;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileHandling _fileHandling;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

            return categories.Select(c =>
            {
                var dto = _mapper.Map<CategoryDTO>(c);
                dto.ImageUrl = GetImageUrlAsync(c.ImageId).Result;
                return dto;
            }).ToList();
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(string id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) throw new ArgumentException("Category not found");

            var categoryDto = _mapper.Map<CategoryDTO>(category);
            categoryDto.ImageUrl =await GetImageUrlAsync(category.ImageId);
            return categoryDto;
        }

        public async Task<CategoryDTO> AddCategoryAsync(AddCategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            if (categoryDto.Image != null)
            {
                category.ImageId = await _fileHandling.UploadFile(categoryDto.Image, await GetPathByName("CategoryImages"));
            }
            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            var categoryDtoResult = _mapper.Map<CategoryDTO>(category);
            categoryDtoResult.ImageUrl = await GetImageUrlAsync(category.ImageId);
            return categoryDtoResult;
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(UpdateCategoryDTO categoryDto)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryDto.Id);
            if (category == null) throw new ArgumentException("Category not found");

            _mapper.Map(categoryDto, category);
            if (categoryDto.Image != null)
            {
                category.ImageId = await _fileHandling.UpdateFile(categoryDto.Image, await GetPathByName("CategoryImages"), category.ImageId);
            }

            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            var categoryDtoResult = _mapper.Map<CategoryDTO>(category);
            categoryDtoResult.ImageUrl = await GetImageUrlAsync(category.ImageId);
            return categoryDtoResult;
        }

        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category == null) throw new ArgumentException("Category not found");

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }

        private async Task<string> GetImageUrlAsync(string imageId)
        {
            if (string.IsNullOrEmpty(imageId)) return null;
            return await _fileHandling.GetFile(imageId);
        }
    }
}
