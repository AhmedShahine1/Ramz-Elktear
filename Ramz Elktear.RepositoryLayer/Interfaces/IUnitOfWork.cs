using Microsoft.AspNetCore.Identity;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Booking;
using Ramz_Elktear.core.Entities.Branchs;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.core.Entities.Offer;

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
        public IBaseRepository<CarModel> CarModelRepository { get; }
        public IBaseRepository<CarCategory> CarCategoryRepository { get; }
        public IBaseRepository<CarColor> CarColorRepository { get; }
        public IBaseRepository<Offer> OfferRepository { get; }
        public IBaseRepository<ImageCar> ImageCarRepository { get; }
        public IBaseRepository<Job> JobRepository { get; }
        public IBaseRepository<Bank> BankRepository { get; }
        public IBaseRepository<City> CityRepository { get; }
        public IBaseRepository<Booking> BookingRepository { get; }

        //-----------------------------------------------------------------------------------
        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
