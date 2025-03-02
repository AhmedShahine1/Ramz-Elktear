using Ramz_Elktear.core.DTO.CategoryModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ISubCategoryService
    {
        Task<IEnumerable<SubCategoryDTO>> GetAllSubCategoriesAsync();
        Task<SubCategoryDTO> GetSubCategoryByIdAsync(string id);
        Task<SubCategoryDTO> AddSubCategoryAsync(CreateSubCategoryDTO subCategoryDto);
        Task<bool> UpdateSubCategoryAsync(UpdateSubCategoryDTO subCategoryDto);
        Task<bool> DeleteSubCategoryAsync(string id);
    }
}
