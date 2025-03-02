namespace Ramz_Elktear.core.DTO.FuelTypeModels
{
    public class FuelTypeDTO
    {
        public string Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string? ImageUrl { get; set; }  // URL for the image
        public bool IsActive { get; set; }
    }
}
