using Microsoft.AspNetCore.Identity;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Offer;
using Ramz_Elktear.core.Entities.Branchs;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.core.Entities.Booking;
using Ramz_Elktear.core.Entities.Color;
using Ramz_Elktear.core.Entities.Categories;
using Ramz_Elktear.core.Entities.Specificate;
using Ramz_Elktear.core.Entities.Promotion;
using Ramz_Elktear.core.Entities;

namespace Ramz_Elktear.RepositoryLayer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<ApplicationUser> UserRepository { get; set; }
        public IBaseRepository<ApplicationRole> RoleRepository { get; set; }
        public IBaseRepository<IdentityUserRole<string>> UserRoleRepository { get; set; }

        public IBaseRepository<Paths> PathsRepository { get; set; }
        public IBaseRepository<Images> ImagesRepository { get; set; }

        public IBaseRepository<Brand> BrandRepository { get; set; }
        public IBaseRepository<Branch> BranchRepository { get; set; }
        public IBaseRepository<Car> CarRepository { get; set; }
        public IBaseRepository<CarColor> CarColorRepository { get; set; }
        public IBaseRepository<CarCategory> CarCategoryRepository { get; set; }
        public IBaseRepository<ImageCar> ImageCarRepository { get; set; }
        public IBaseRepository<Job> JobRepository { get; set; }
        public IBaseRepository<Bank> BankRepository { get; set; }
        public IBaseRepository<Booking> BookingRepository { get; set; }
        public IBaseRepository<Category> CategoryRepository { get; set; }
        public IBaseRepository<SubCategory> SubCategoryRepository { get; set; }
        public IBaseRepository<Option> OptionRepository { get; set; }
        public IBaseRepository<Offers> OffersRepository { get; set; }
        public IBaseRepository<TransmissionType> TransmissionTypeRepository { get; set; }
        public IBaseRepository<FuelType> FuelTypeRepository { get; set; }
        public IBaseRepository<EngineSize> EngineSizeRepository { get; set; }
        public IBaseRepository<Origin> OriginRepository { get; set; }
        public IBaseRepository<ModelYear> ModelYearRepository { get; set; }
        public IBaseRepository<EnginePosition> EnginePositionRepository { get; set; }
        public IBaseRepository<Colors> ColorsRepository { get; set; }
        public IBaseRepository<CarOffer> CarOfferRepository { get; set; }
        public IBaseRepository<CarSpecification> CarSpecificationRepository { get; set; }
        public IBaseRepository<Specification> SpecificationRepository { get; set; }
        public IBaseRepository<Promotion> PromotionRepository { get; set; }
        public IBaseRepository<InsurancePercentage> InsurancePercentageRepository { get; set; }
        public IBaseRepository<InstallmentRequest> InstallmentRequestsRepository { get; set; }
        public IBaseRepository<ContactForm> ContactFormsRepository { get; set; }
        public IBaseRepository<Setting> SettingsRepository { get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            UserRepository = new BaseRepository<ApplicationUser>(context);
            RoleRepository = new BaseRepository<ApplicationRole>(context);
            UserRoleRepository = new BaseRepository<IdentityUserRole<string>>(context);
            PathsRepository = new BaseRepository<Paths>(context);
            ImagesRepository = new BaseRepository<Images>(context);
            BrandRepository = new BaseRepository<Brand>(context);
            BranchRepository = new BaseRepository<Branch>(context);
            CarRepository = new BaseRepository<Car>(context);
            CarCategoryRepository = new BaseRepository<CarCategory>(context);
            CarColorRepository = new BaseRepository<CarColor>(context);
            ImageCarRepository = new BaseRepository<ImageCar>(context);
            JobRepository = new BaseRepository<Job>(context);
            BankRepository = new BaseRepository<Bank>(context);
            BookingRepository = new BaseRepository<Booking>(context);
            CategoryRepository = new BaseRepository<Category>(context);
            SubCategoryRepository = new BaseRepository<SubCategory>(context);
            OptionRepository = new BaseRepository<Option>(context);
            TransmissionTypeRepository = new BaseRepository<TransmissionType>(context);
            FuelTypeRepository = new BaseRepository<FuelType>(context);
            EngineSizeRepository = new BaseRepository<EngineSize>(context);
            OriginRepository = new BaseRepository<Origin>(context);
            OffersRepository = new BaseRepository<Offers>(context);
            ModelYearRepository = new BaseRepository<ModelYear>(context);
            EnginePositionRepository = new BaseRepository<EnginePosition>(context);
            ColorsRepository = new BaseRepository<Colors>(context);
            CarOfferRepository = new BaseRepository<CarOffer>(context);
            CarSpecificationRepository = new BaseRepository<CarSpecification>(context);
            SpecificationRepository = new BaseRepository<Specification>(context);
            PromotionRepository = new BaseRepository<Promotion>(context);
            InsurancePercentageRepository = new BaseRepository<InsurancePercentage>(context);
            InstallmentRequestsRepository = new BaseRepository<InstallmentRequest>(context);
            ContactFormsRepository = new BaseRepository<ContactForm>(context);
            SettingsRepository = new BaseRepository<Setting>(context);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
