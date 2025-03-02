using Ramz_Elktear.core.DTO.BrandModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandDetails>> GetAllBrandsAsync();
        Task<BrandDetails> GetBrandByIdAsync(string brandId);
        Task<BrandDetails> AddBrandAsync(AddBrand brandDto);
        Task<BrandDetails> UpdateBrandAsync(UpdateBrandDto brandDto);
        Task<bool> DeleteBrandAsync(string brandId);
    }
}
