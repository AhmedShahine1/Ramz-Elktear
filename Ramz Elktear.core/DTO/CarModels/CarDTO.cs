using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.CategoryModels;
using Ramz_Elktear.core.DTO.ColorModels;
using Ramz_Elktear.core.DTO.EnginePositionModels;
using Ramz_Elktear.core.DTO.EngineSizeModels;
using Ramz_Elktear.core.DTO.FuelTypeModels;
using Ramz_Elktear.core.DTO.ModelYearModels;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.DTO.OptionModels;
using Ramz_Elktear.core.DTO.OriginModels;
using Ramz_Elktear.core.DTO.SpecificationModels;
using Ramz_Elktear.core.DTO.TransmissionTypeModels;

namespace Ramz_Elktear.core.DTO.CarModels
{
    public class CarDTO
    {
        public string Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescrptionAr { get; set; }
        public string DescrptionEn { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal InstallmentPrice { get; set; }
        public int QuantityInStock { get; set; }
        public SubCategoryDTO SubCategory { get; set; }
        public string CarCode { get; set; }
        public string CarSKU { get; set; }
        public string ImageUrl { get; set; }
        public string ImageWithoutBackgroundUrl { get; set; }
        public BrandDetails Brand { get; set; }
        public OptionDTO Option { get; set; }
        public TransmissionTypeDTO TransmissionType { get; set; }
        public FuelTypeDTO FuelType { get; set; }
        public EngineSizeDTO EngineSize { get; set; }
        public OriginDTO Origin { get; set; }
        public ModelYearDTO ModelYear { get; set; }
        public List<string> ImagesUrl { get; set; } = new List<string>();
        public List<string> InsideCarImagesUrl { get; set; } = new List<string>();
        public List<OfferDTO> Offer { get; set; } = new List<OfferDTO>();
        public List<ColorDTO> Color { get; set; } = new List<ColorDTO>();
        public List<SpecificationDTO> Specifications { get; set; } = new List<SpecificationDTO>();
        public int? Kilometers { get; set; }
        public EnginePositionDTO EnginePosition { get; set; }
        public bool IsSpecial { get; set; }
        public bool IsActive { get; set; }
    }
}