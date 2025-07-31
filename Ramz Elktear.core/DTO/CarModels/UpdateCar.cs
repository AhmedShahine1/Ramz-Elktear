using Microsoft.AspNetCore.Http;
namespace Ramz_Elktear.core.DTO.CarModels
{
    public class UpdateCarDTO
    {
        public string Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescrptionAr { get; set; }
        public string DescrptionEn { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal InstallmentPrice { get; set; }
        public int QuantityInStock { get; set; }
        public string SubCategoryId { get; set; }
        public string CarCode { get; set; }
        public string CarSKU { get; set; }
        public IFormFile Image { get; set; }
        public IFormFile ImageWithoutBackground { get; set; }
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
        public List<IFormFile> InsideCarImages { get; set; } = new List<IFormFile>();
        public string? ImageURL { get; set; }
        public string? ImageWithoutBackgroundURL { get; set; }
        public List<string> ImagesURL { get; set; } = new List<string>();
        public List<string> InsideCarImagesURL { get; set; } = new List<string>();
        public List<string> OfferId { get; set; } = new List<string>();
        public List<string> ColorId { get; set; } = new List<string>();
        public List<string> SpecificationsId { get; set; } = new List<string>();
        public string BrandId { get; set; }
        public string OptionId { get; set; }
        public string TransmissionTypeId { get; set; }
        public string FuelTypeId { get; set; }
        public string EngineSizeId { get; set; }
        public string OriginId { get; set; }
        public string ModelYearId { get; set; }
        public int? Kilometers { get; set; }
        public string EnginePositionId { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsActive { get; set; }

        // Add support for color images in updates
        public List<ColorImageModel> ColorImages { get; set; } = new();
    }
}