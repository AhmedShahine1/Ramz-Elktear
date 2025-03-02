namespace Ramz_Elktear.core.DTO.EnginePositionModels
{
    public class EnginePositionDTO
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
