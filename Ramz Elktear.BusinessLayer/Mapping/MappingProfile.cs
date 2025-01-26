using AutoMapper;
using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.DTO.BranchModels;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.CityModels;
using Ramz_Elktear.core.DTO.FileModels;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.DTO.RegisterModels;
using Ramz_Elktear.core.DTO.RoleModels;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Booking;
using Ramz_Elktear.core.Entities.Branchs;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Installment;

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
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
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
            //------------------------------------------------------------------------------------------------------
            CreateMap<AddCar, Car>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Brands, opt => opt.Ignore()) // Brand is fetched separately
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type)) // Make sure Type is mapped
                .ForMember(dest => dest.NumberOfSeats, opt => opt.MapFrom(src => src.NumberOfSeats))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new CarCategory { Id = src.Category }))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => new CarModel { Id = src.ModelId }))
                .ForMember(dest => dest.Engine, opt => opt.MapFrom(src => src.Engine))
                .ForMember(dest => dest.Transmission, opt => opt.MapFrom(src => src.Transmission))
                .ForMember(dest => dest.Acceleration, opt => opt.MapFrom(src => src.Acceleration))
                .ForMember(dest => dest.FuelConsumption, opt => opt.MapFrom(src => src.FuelConsumption))
                .ForMember(dest => dest.Dimensions, opt => opt.MapFrom(src => src.Dimensions))
                .ForMember(dest => dest.StorageCapacity, opt => opt.MapFrom(src => src.StorageCapacity))
                .ForMember(dest => dest.KeyFeatures, opt => opt.MapFrom(src => src.KeyFeatures))
                .ForMember(dest => dest.SafetySystems, opt => opt.MapFrom(src => src.SafetySystems))
                .ForMember(dest => dest.FuelType, opt => opt.MapFrom(src => src.FuelType))
                .ForMember(dest => dest.TransmissionType, opt => opt.MapFrom(src => src.Transmission))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.AvailableColors, opt => opt.MapFrom(src => src.AvailableColors.Select(id => new CarColor { Id = id }).ToList()));

            // Mapping for car details
            CreateMap<Car, CarDetails>()
                        .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brands.Name))  // Assuming Brand entity has a Name property
                        .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                        .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model.Name)) // Assuming Model entity has a Name property
                        .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name)) // Assuming Category entity has a Name property
                        .ForMember(dest => dest.AvailableColors, opt => opt.MapFrom(src => src.AvailableColors.Select(c => c.Name).ToList())) // Assuming Color entity has a Name property
                        .ForMember(dest => dest.KeyFeatures, opt => opt.MapFrom(src => src.KeyFeatures))
                        .ForMember(dest => dest.SafetySystems, opt => opt.MapFrom(src => src.SafetySystems))
                        .ForMember(dest => dest.ImageUrls, opt => opt.Ignore()); // Images will be added separately in service
            // CarModel to DTO
            CreateMap<CarModel, CarModel>();

            // CarCategory to DTO
            CreateMap<CarCategory, CarCategory>();

            // CarColor to DTO
            CreateMap<CarColor, CarColor>();

            // Bank Mappings
            CreateMap<AddBank, Bank>().ReverseMap();
            CreateMap<Bank, BankDetails>().ReverseMap();

            // Job Mappings
            CreateMap<AddJob, Job>().ReverseMap();
            CreateMap<Job, JobDetails>().ReverseMap();

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
        }
    }
}
