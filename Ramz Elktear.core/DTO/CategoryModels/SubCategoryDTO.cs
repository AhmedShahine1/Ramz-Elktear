using Ramz_Elktear.core.DTO.BrandModels;

namespace Ramz_Elktear.core.DTO.CategoryModels
{
    public class SubCategoryDTO
    {
        public string Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public CategoryDTO Category { get; set; }
        public BrandDetails Brand { get; set; }
        public bool IsActive { get; set; }
    }
}
