using Ramz_Elktear.core.DTO.CategoryModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(string id);
        Task<CategoryDTO> AddCategoryAsync(AddCategoryDTO categoryDto);
        Task<CategoryDTO> UpdateCategoryAsync(UpdateCategoryDTO categoryDto);
        Task<bool> DeleteCategoryAsync(string id);
    }
}
