using Microsoft.AspNetCore.Identity;
using Ramz_Elktear.core.Entities;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Booking;
using Ramz_Elktear.core.Entities.Branchs;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Categories;
using Ramz_Elktear.core.Entities.Color;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.core.Entities.Localization;
using Ramz_Elktear.core.Entities.Offer;
using Ramz_Elktear.core.Entities.Promotion;
using Ramz_Elktear.core.Entities.Specificate;

namespace Ramz_Elktear.RepositoryLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IBaseRepository<ApplicationUser> UserRepository { get; }
        public IBaseRepository<ApplicationRole> RoleRepository { get; }
        public IBaseRepository<IdentityUserRole<string>> UserRoleRepository { get; }
        public IBaseRepository<Paths> PathsRepository { get; }
        public IBaseRepository<Images> ImagesRepository { get; }
        public IBaseRepository<Brand> BrandRepository { get; }
        public IBaseRepository<Branch> BranchRepository { get; }
        public IBaseRepository<Car> CarRepository { get; }
        public IBaseRepository<CarCategory> CarCategoryRepository { get; }
        public IBaseRepository<CarColor> CarColorRepository { get; }
        public IBaseRepository<ImageCar> ImageCarRepository { get; }
        public IBaseRepository<Job> JobRepository { get; }
        public IBaseRepository<Bank> BankRepository { get; }
        public IBaseRepository<Booking> BookingRepository { get; }
        public IBaseRepository<Category> CategoryRepository { get; }
        public IBaseRepository<SubCategory> SubCategoryRepository { get; }
        public IBaseRepository<Option> OptionRepository { get; }
        public IBaseRepository<Offers> OffersRepository { get; }
        public IBaseRepository<TransmissionType> TransmissionTypeRepository { get; }
        public IBaseRepository<FuelType> FuelTypeRepository { get; }
        public IBaseRepository<EngineSize> EngineSizeRepository { get; }
        public IBaseRepository<Origin> OriginRepository { get; }
        public IBaseRepository<ModelYear> ModelYearRepository { get; }
        public IBaseRepository<EnginePosition> EnginePositionRepository { get; }
        public IBaseRepository<Colors> ColorsRepository { get; }
        public IBaseRepository<CarOffer> CarOfferRepository { get; }
        public IBaseRepository<CarSpecification> CarSpecificationRepository { get; }
        public IBaseRepository<Specification> SpecificationRepository { get; }
        public IBaseRepository<Promotion> PromotionRepository { get; }
        public IBaseRepository<InsurancePercentage> InsurancePercentageRepository { get; }
        public IBaseRepository<InstallmentRequest> InstallmentRequestsRepository { get; }
        public IBaseRepository<ContactForm> ContactFormsRepository { get; }
        public IBaseRepository<Setting> SettingsRepository { get; }
        public IBaseRepository<LocalizationResource> LocalizationResourceRepository { get; }
        public IBaseRepository<LocalizationChangeLog> LocalizationChangeLogRepository { get; }

        //-----------------------------------------------------------------------------------
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
