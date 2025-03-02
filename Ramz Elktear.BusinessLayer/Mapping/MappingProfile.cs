using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.DTO.BranchModels;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.CarColorModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.CarOfferModels;
using Ramz_Elktear.core.DTO.CarSpecificationModels;
using Ramz_Elktear.core.DTO.CategoryModels;
using Ramz_Elktear.core.DTO.CityModels;
using Ramz_Elktear.core.DTO.ColorModels;
using Ramz_Elktear.core.DTO.EnginePositionModels;
using Ramz_Elktear.core.DTO.EngineSizeModels;
using Ramz_Elktear.core.DTO.FileModels;
using Ramz_Elktear.core.DTO.FuelTypeModels;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.DTO.ModelYearModels;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.DTO.OptionModels;
using Ramz_Elktear.core.DTO.OriginModels;
using Ramz_Elktear.core.DTO.RegisterModels;
using Ramz_Elktear.core.DTO.RoleModels;
using Ramz_Elktear.core.DTO.SpecificationModels;
using Ramz_Elktear.core.DTO.TransmissionTypeModels;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Booking;
using Ramz_Elktear.core.Entities.Branchs;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Categories;
using Ramz_Elktear.core.Entities.Color;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.core.Entities.Offer;
using Ramz_Elktear.core.Entities.Specificate;

namespace Ramz_Elktear.BusinessLayer.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //--------------------------------------------------------------------------------------------------------
            // Mapping for RoleDTO <-> ApplicationRole
            CreateMap<RoleDTO, ApplicationRole>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.RoleName))
                .ForMember(dest => dest.ArName, opt => opt.MapFrom(src => src.RoleAr))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.RoleDescription))
                .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for Paths <-> PathsModel
            CreateMap<Paths, PathsModel>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ReverseMap();

            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> RegisterAdmin
            CreateMap<ApplicationUser, RegisterAdmin>()
                    .ReverseMap()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));

            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> RegisterSales
            CreateMap<ApplicationUser, RegisterSales>()
                    .ReverseMap()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));

            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> RegisterSupportDeveloper
            CreateMap<ApplicationUser, RegisterSupportDeveloper>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));
            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> RegisterCustomer
            CreateMap<ApplicationUser, RegisterCustomer>()
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PhoneNumber));
            //--------------------------------------------------------------------------------------------------------
            // Mapping for ApplicationUser <-> AuthDTO
            CreateMap<ApplicationUser, AuthDTO>()
                .ReverseMap();

            // CarColor to DTO
            CreateMap<CarColor, CarColor>();

            // Bank Mappings
            CreateMap<AddBank, Bank>().ReverseMap();
            CreateMap<Bank, BankDetails>().ReverseMap();

            // Job Mappings
            CreateMap<AddJob, Job>().ReverseMap();
            CreateMap<Job, JobDetails>().ReverseMap();
            CreateMap<Job, UpdateJob>().ReverseMap();

            // Branch Mappings
            CreateMap<AddBranch, Branch>().ReverseMap();
            CreateMap<Branch, BranchDetails>().ReverseMap();

            // Brand Mappings
            CreateMap<AddBrand, Brand>().ReverseMap();
            CreateMap<Brand, BrandDetails>().ReverseMap();

            // City mappings
            CreateMap<City, CityDto>().ReverseMap();
            CreateMap<City, AddCityDto>().ReverseMap();
            CreateMap<City, UpdateCityDto>().ReverseMap();
            CreateMap<Booking, CreateBookingDto>().ReverseMap();

            // Mapping from ModelYear to ModelYearDTO
            CreateMap<ModelYear, ModelYearDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // Mapping from ModelYearDTO to ModelYear
            CreateMap<ModelYearDTO, ModelYear>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())  // Ignore CreatedDate during mapping
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())  // Ignore LastModifiedDate during mapping
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())  // Ignore IsDeleted during mapping
                .ForMember(dest => dest.Cars, opt => opt.Ignore());  // Ignore the Cars collection
                                                                     // Mapping from Option Entity to OptionDTO
            CreateMap<Option, OptionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            // Mapping from OptionDTO to Option Entity
            CreateMap<OptionDTO, Option>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            CreateMap<Origin, OriginDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
            .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr))
            .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<OriginDTO, Origin>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Id)))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescriptionAr, opt => opt.MapFrom(src => src.DescriptionAr))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Cars, opt => opt.Ignore());

            CreateMap<FuelType, FuelTypeDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<FuelTypeDTO, FuelType>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // Mapping for EnginePosition
            CreateMap<EnginePosition, EnginePositionDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<EnginePositionDTO, EnginePosition>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // Mapping for EngineSize
            CreateMap<EngineSize, EngineSizeDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<EngineSizeDTO, EngineSize>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<ColorDTO, Colors>();
            CreateMap<Colors, ColorDTO>();

            // Mapping for Category
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<AddCategoryDTO, Category>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<Category, AddCategoryDTO>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<Category, UpdateCategoryDTO>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<CategoryDTO, Category>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<SubCategory, SubCategoryDTO>();

            CreateMap<SubCategoryDTO, SubCategory>();

            CreateMap<SubCategory, CreateSubCategoryDTO>();

            CreateMap<CreateSubCategoryDTO, SubCategory>();

            CreateMap<SubCategory, UpdateSubCategoryDTO>();

            CreateMap<UpdateSubCategoryDTO, SubCategory>();

            CreateMap<SpecificationDTO, Specification>();

            CreateMap<Specification, SpecificationDTO>();

            CreateMap<TransmissionType, TransmissionTypeDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<TransmissionTypeDTO, TransmissionType>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // AddCar to CarDTO
            CreateMap<AddCar, CarDTO>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescrptionAr, opt => opt.MapFrom(src => src.DescrptionAr))
                .ForMember(dest => dest.DescrptionEn, opt => opt.MapFrom(src => src.DescrptionEn))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.InstallmentPrice, opt => opt.MapFrom(src => src.InstallmentPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.CarCode, opt => opt.MapFrom(src => src.CarCode))
                .ForMember(dest => dest.CarSKU, opt => opt.MapFrom(src => src.CarSKU))
                .ForMember(dest => dest.ImagesUrl, opt => opt.MapFrom(src => src.Images.Select(i => i.FileName).ToList())) // Assuming you're storing file names in Images
                .ForMember(dest => dest.Offer, opt => opt.MapFrom(src => src.OfferId))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.Specifications, opt => opt.MapFrom(src => src.SpecificationsId))
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => new BrandDetails { Id = src.BrandId })) // Assuming you're populating the Brand object later
                .ForMember(dest => dest.Option, opt => opt.MapFrom(src => new OptionDTO { Id = src.OptionId })) // Same for Option
                .ForMember(dest => dest.TransmissionType, opt => opt.MapFrom(src => new TransmissionTypeDTO { Id = src.TransmissionTypeId }))
                .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => new FuelTypeDTO { Id = src.FuelTypeId }))
                .ForMember(dest => dest.EngineSize, opt => opt.MapFrom(src => new EngineSizeDTO { Id = src.EngineSizeId }))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => new OriginDTO { Id = src.OriginId }))
                .ForMember(dest => dest.ModelYear, opt => opt.MapFrom(src => new ModelYearDTO { Id = src.ModelYearId }))
                .ForMember(dest => dest.EnginePosition, opt => opt.MapFrom(src => new EnginePositionDTO { Id = src.EnginePositionId }))
                .ForMember(dest => dest.Kilometers, opt => opt.MapFrom(src => src.Kilometers))
                .ForMember(dest => dest.IsSpecial, opt => opt.MapFrom(src => src.IsSpecial))
                .ReverseMap();

            // CarDTO to AddCar
            CreateMap<CarDTO, AddCar>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescrptionAr, opt => opt.MapFrom(src => src.DescrptionAr))
                .ForMember(dest => dest.DescrptionEn, opt => opt.MapFrom(src => src.DescrptionEn))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.InstallmentPrice, opt => opt.MapFrom(src => src.InstallmentPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.CarCode, opt => opt.MapFrom(src => src.CarCode))
                .ForMember(dest => dest.CarSKU, opt => opt.MapFrom(src => src.CarSKU))
                .ForMember(dest => dest.Images, opt => opt.Ignore()) // Assuming you're handling file conversions
                .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.Offer.Select(o => o.Id).ToList())) // Assuming Offer is a list of OfferDTO objects
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.Color.Select(c => c.Id).ToList())) // Same for Color
                .ForMember(dest => dest.SpecificationsId, opt => opt.MapFrom(src => src.Specifications))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.Brand.Id)) // Assuming Brand has an Id property
                .ForMember(dest => dest.OptionId, opt => opt.MapFrom(src => src.Option.Id)) // Same for Option
                .ForMember(dest => dest.TransmissionTypeId, opt => opt.MapFrom(src => src.TransmissionType.Id))
                .ForMember(dest => dest.FuelTypeId, opt => opt.MapFrom(src => src.FuelType.Id))
                .ForMember(dest => dest.EngineSizeId, opt => opt.MapFrom(src => src.EngineSize.Id))
                .ForMember(dest => dest.OriginId, opt => opt.MapFrom(src => src.Origin.Id))
                .ForMember(dest => dest.ModelYearId, opt => opt.MapFrom(src => src.ModelYear.Id))
                .ForMember(dest => dest.EnginePositionId, opt => opt.MapFrom(src => src.EnginePosition.Id))
                .ForMember(dest => dest.Kilometers, opt => opt.MapFrom(src => src.Kilometers))
                .ForMember(dest => dest.IsSpecial, opt => opt.MapFrom(src => src.IsSpecial));

            // Mapping from AddCar to Car
            CreateMap<AddCar, Car>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescrptionAr, opt => opt.MapFrom(src => src.DescrptionAr))
                .ForMember(dest => dest.DescrptionEn, opt => opt.MapFrom(src => src.DescrptionEn))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.InstallmentPrice, opt => opt.MapFrom(src => src.InstallmentPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.CarCode, opt => opt.MapFrom(src => src.CarCode))
                .ForMember(dest => dest.CarSKU, opt => opt.MapFrom(src => src.CarSKU))
                .ForMember(dest => dest.Image, opt => opt.Ignore()) // Assuming you're handling file names and other properties
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.OptionId, opt => opt.MapFrom(src => src.OptionId))
                .ForMember(dest => dest.TransmissionTypeId, opt => opt.MapFrom(src => src.TransmissionTypeId))
                .ForMember(dest => dest.FuelTypeId, opt => opt.MapFrom(src => src.FuelTypeId))
                .ForMember(dest => dest.EngineSizeId, opt => opt.MapFrom(src => src.EngineSizeId))
                .ForMember(dest => dest.OriginId, opt => opt.MapFrom(src => src.OriginId))
                .ForMember(dest => dest.ModelYearId, opt => opt.MapFrom(src => src.ModelYearId))
                .ForMember(dest => dest.EnginePositionId, opt => opt.MapFrom(src => src.EnginePositionId))
                .ForMember(dest => dest.Kilometers, opt => opt.MapFrom(src => src.Kilometers))
                .ForMember(dest => dest.IsSpecial, opt => opt.MapFrom(src => src.IsSpecial));

            // Mapping from Car to AddCar
            CreateMap<Car, AddCar>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescrptionAr, opt => opt.MapFrom(src => src.DescrptionAr))
                .ForMember(dest => dest.DescrptionEn, opt => opt.MapFrom(src => src.DescrptionEn))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.InstallmentPrice, opt => opt.MapFrom(src => src.InstallmentPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.CarCode, opt => opt.MapFrom(src => src.CarCode))
                .ForMember(dest => dest.CarSKU, opt => opt.MapFrom(src => src.CarSKU))
                .ForMember(dest => dest.Image, opt => opt.Ignore()) // Assuming you want to keep a single image
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.OptionId, opt => opt.MapFrom(src => src.OptionId))
                .ForMember(dest => dest.TransmissionTypeId, opt => opt.MapFrom(src => src.TransmissionTypeId))
                .ForMember(dest => dest.FuelTypeId, opt => opt.MapFrom(src => src.FuelTypeId))
                .ForMember(dest => dest.EngineSizeId, opt => opt.MapFrom(src => src.EngineSizeId))
                .ForMember(dest => dest.OriginId, opt => opt.MapFrom(src => src.OriginId))
                .ForMember(dest => dest.ModelYearId, opt => opt.MapFrom(src => src.ModelYearId))
                .ForMember(dest => dest.EnginePositionId, opt => opt.MapFrom(src => src.EnginePositionId))
                .ForMember(dest => dest.Kilometers, opt => opt.MapFrom(src => src.Kilometers))
                .ForMember(dest => dest.IsSpecial, opt => opt.MapFrom(src => src.IsSpecial));

            // Mapping from CarDTO to Car
            CreateMap<CarDTO, Car>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescrptionAr, opt => opt.MapFrom(src => src.DescrptionAr))
                .ForMember(dest => dest.DescrptionEn, opt => opt.MapFrom(src => src.DescrptionEn))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.InstallmentPrice, opt => opt.MapFrom(src => src.InstallmentPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.CarCode, opt => opt.MapFrom(src => src.CarCode))
                .ForMember(dest => dest.CarSKU, opt => opt.MapFrom(src => src.CarSKU))
                .ForMember(dest => dest.Image, opt => opt.Ignore()) // Assuming you're using File entity for images
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.Brand.Id))
                .ForMember(dest => dest.OptionId, opt => opt.MapFrom(src => src.Option.Id))
                .ForMember(dest => dest.TransmissionTypeId, opt => opt.MapFrom(src => src.TransmissionType.Id))
                .ForMember(dest => dest.FuelTypeId, opt => opt.MapFrom(src => src.FuelType.Id))
                .ForMember(dest => dest.EngineSizeId, opt => opt.MapFrom(src => src.EngineSize.Id))
                .ForMember(dest => dest.OriginId, opt => opt.MapFrom(src => src.Origin.Id))
                .ForMember(dest => dest.ModelYearId, opt => opt.MapFrom(src => src.ModelYear.Id))
                .ForMember(dest => dest.EnginePositionId, opt => opt.MapFrom(src => src.EnginePosition.Id))
                .ForMember(dest => dest.Kilometers, opt => opt.MapFrom(src => src.Kilometers))
                .ForMember(dest => dest.IsSpecial, opt => opt.MapFrom(src => src.IsSpecial));

            // Mapping from Car to CarDTO
            CreateMap<Car, CarDTO>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DescrptionAr, opt => opt.MapFrom(src => src.DescrptionAr))
                .ForMember(dest => dest.DescrptionEn, opt => opt.MapFrom(src => src.DescrptionEn))
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.InstallmentPrice, opt => opt.MapFrom(src => src.InstallmentPrice))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.CarCode, opt => opt.MapFrom(src => src.CarCode))
                .ForMember(dest => dest.CarSKU, opt => opt.MapFrom(src => src.CarSKU))
                .ForMember(dest => dest.ImagesUrl, opt => opt.Ignore()) // Mapping images as URLs
                .ForMember(dest => dest.Option, opt => opt.MapFrom(src => new OptionDTO { Id = src.OptionId }))
                .ForMember(dest => dest.TransmissionType, opt => opt.MapFrom(src => new TransmissionTypeDTO { Id = src.TransmissionTypeId }))
                .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => new FuelTypeDTO { Id = src.FuelTypeId }))
                .ForMember(dest => dest.EngineSize, opt => opt.MapFrom(src => new EngineSizeDTO { Id = src.EngineSizeId }))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => new OriginDTO { Id = src.OriginId }))
                .ForMember(dest => dest.ModelYear, opt => opt.MapFrom(src => new ModelYearDTO { Id = src.ModelYearId }))
                .ForMember(dest => dest.EnginePosition, opt => opt.MapFrom(src => new EnginePositionDTO { Id = src.EnginePositionId }))
                .ForMember(dest => dest.Kilometers, opt => opt.MapFrom(src => src.Kilometers))
                .ForMember(dest => dest.IsSpecial, opt => opt.MapFrom(src => src.IsSpecial));

            // Mapping from CarOfferDTO to CarOffer (Entity)
            CreateMap<CarOfferDTO, CarOffer>()
                .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.OfferId))
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Ignore audit fields
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Offer, opt => opt.Ignore()) // Ignore Offer complex object
                .ForMember(dest => dest.Car, opt => opt.Ignore()); // Ignore Car complex object

            // Mapping from CarOffer to CarOfferDTO
            CreateMap<CarOffer, CarOfferDTO>()
                .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.OfferId))
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // Mapping from CarColorDTO to CarColor (Entity)
            CreateMap<CarColorDTO, CarColor>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Color, opt => opt.Ignore()) // Ignore Color as it's a complex object
                .ForMember(dest => dest.Car, opt => opt.Ignore()) // Ignore Car as it's a complex object
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Ignore audit fields
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // Mapping from CarColor to CarColorDTO
            CreateMap<CarColor, CarColorDTO>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.ColorId, opt => opt.MapFrom(src => src.ColorId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            // Mapping from CarSpecificationDTO to CarSpecification (Entity)
            CreateMap<CarSpecificationDTO, CarSpecification>()
                .ForMember(dest => dest.carId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.SpecificationId, opt => opt.MapFrom(src => src.SpecificationId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Ignore audit fields
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.car, opt => opt.Ignore()) // Ignore Car complex object
                .ForMember(dest => dest.Specification, opt => opt.Ignore()); // Ignore Specification complex object

            // Mapping from CarSpecification to CarSpecificationDTO
            CreateMap<CarSpecification, CarSpecificationDTO>()
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.carId))
                .ForMember(dest => dest.SpecificationId, opt => opt.MapFrom(src => src.SpecificationId))
                .ForMember(dest => dest.SpecificationName, opt => opt.MapFrom(src => src.Specification.NameAr)) // Assuming Specification has a Name property
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
            // Mapping from UpdateOffer DTO to Offers entity
            CreateMap<UpdateOffer, Offers>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.NewPrice))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.delivery, opt => opt.MapFrom(src => src.Delivery))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now)) // Assuming current date on creation
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.Now)) // Same for last modified
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)) // Assuming the offer is active on creation
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)); // Assuming offer is not deleted on creation
                                                                                      // Mapping from Offers entity to OfferDTO
            CreateMap<Offers, OfferDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.NewPrice))
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore()) // Mapping from Image to ImageUrl
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.LastModifiedBy))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.LastModifiedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Delivery, opt => opt.MapFrom(src => src.delivery));

            // Mapping from OfferDTO to Offers entity (for updating or creating)
            CreateMap<OfferDTO, Offers>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.NewPrice))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.LastModifiedBy, opt => opt.MapFrom(src => src.LastModifiedBy))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => src.LastModifiedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.delivery, opt => opt.MapFrom(src => src.Delivery));
            // Mapping from AddOffer (DTO) to Offers (Entity)
            CreateMap<AddOffer, Offers>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.NewPrice))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.delivery, opt => opt.MapFrom(src => src.Delivery))
                // Handle Image mapping (image is an IFormFile in AddOffer and an Images entity in Offers)
                .ForMember(dest => dest.Image, opt => opt.Ignore()) // Image needs to be handled separately
                .ForMember(dest => dest.ImageId, opt => opt.Ignore()) // ImageId should be set separately
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // Mapping from Offers (Entity) to AddOffer (DTO)
            CreateMap<Offers, AddOffer>()
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.NewPrice, opt => opt.MapFrom(src => src.NewPrice))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.Delivery, opt => opt.MapFrom(src => src.delivery))
                // Image mapping, just to pass the URL from the Image entity if needed in the DTO
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // Mapping Car entity to CarDetails DTO
            CreateMap<Car, CarDetails>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.NameEn))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Option.NameEn))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.SubCategory.NameEn))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.Engine, opt => opt.MapFrom(src => src.EngineSize.NameEn))
                .ForMember(dest => dest.Transmission, opt => opt.MapFrom(src => src.TransmissionType.NameEn))
                .ForMember(dest => dest.FuelConsumption, opt => opt.MapFrom(src => src.FuelType.NameEn))
                .ForMember(dest => dest.KeyFeatures, opt => opt.MapFrom(src => src.CarOffers.Select(o => o.Offer.NameEn).ToList())) // Assuming there's an Offer name in CarOffers
                .ForMember(dest => dest.SafetySystems, opt => opt.Ignore()) // Assuming CarSpecifications has safety system info
                .ForMember(dest => dest.AvailableColors, opt => opt.MapFrom(src => src.CarColors.Select(c => c.Color.Name).ToList())) // Assuming CarColor contains the Color entity
                .ForMember(dest => dest.ImageUrls, opt => opt.Ignore()) // Assuming ImageCar contains Image with Url property
                .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType.NameEn))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.SellingPrice))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.DescrptionEn)); // Assuming Description is in En language

            CreateMap<CarDetails, Car>()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.Model)) // Assuming Model is mapped to NameEn
                .ForMember(dest => dest.OptionId, opt => opt.MapFrom(src => src.Type)) // Assuming Type refers to Option
                .ForMember(dest => dest.SubCategoryId, opt => opt.MapFrom(src => src.Category)) // Assuming Category refers to SubCategory
                .ForMember(dest => dest.EngineSizeId, opt => opt.MapFrom(src => src.Engine)) // Assuming Engine refers to EngineSize
                .ForMember(dest => dest.TransmissionTypeId, opt => opt.MapFrom(src => src.Transmission)) // Assuming Transmission refers to TransmissionType
                .ForMember(dest => dest.FuelTypeId, opt => opt.MapFrom(src => src.FuelConsumption)) // Assuming FuelConsumption refers to FuelType
                .ForMember(dest => dest.SellingPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.DescrptionEn, opt => opt.MapFrom(src => src.Description)) // Assuming Description in English
                .ForMember(dest => dest.CarColors, opt => opt.Ignore()) // CarColors needs to be handled separately, based on how colors are added to Car
                .ForMember(dest => dest.CarImages, opt => opt.Ignore()) // CarImages needs to be handled separately
                .ForMember(dest => dest.CarOffers, opt => opt.Ignore()) // CarOffers need to be mapped separately if necessary
                .ForMember(dest => dest.CarSpecifications, opt => opt.Ignore()) // CarSpecifications need to be mapped separately
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)) // Default as true or from a field in CarDetails
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)) // Default as false or from a field in CarDetails
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Typically handled by system
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) // Typically handled by system
                .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore()) // Typically handled by system
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore()); // Typically handled by system

        }
    }
}
