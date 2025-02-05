﻿using Microsoft.AspNetCore.Identity;
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
        public IBaseRepository<CarModel> CarModelRepository { get; set; }
        public IBaseRepository<ImageCar> ImageCarRepository { get; set; }
        public IBaseRepository<Offer> OfferRepository { get; set; }
        public IBaseRepository<Job> JobRepository { get; set; }
        public IBaseRepository<Bank> BankRepository { get; set; }
        public IBaseRepository<City> CityRepository { get; set; }
        public IBaseRepository<Booking> BookingRepository { get; set; }

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
            CarModelRepository = new BaseRepository<CarModel>(context);
            CarCategoryRepository = new BaseRepository<CarCategory>(context);
            CarColorRepository = new BaseRepository<CarColor>(context);
            ImageCarRepository = new BaseRepository<ImageCar>(context);
            OfferRepository = new BaseRepository<Offer>(context);
            JobRepository = new BaseRepository<Job>(context);
            BankRepository = new BaseRepository<Bank>(context);
            CityRepository = new BaseRepository<City>(context);
            BookingRepository = new BaseRepository<Booking>(context);
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
