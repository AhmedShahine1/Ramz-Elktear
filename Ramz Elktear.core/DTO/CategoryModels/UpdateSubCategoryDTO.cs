namespace Ramz_Elktear.core.DTO.CategoryModels
{
    public class UpdateSubCategoryDTO
    {
        public string Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string CategoryId { get; set; }
        public string BrandId { get; set; }
        public bool IsActive { get; set; }
    }
}
