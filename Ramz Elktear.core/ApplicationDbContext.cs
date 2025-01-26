using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Brands;
using Ramz_Elktear.core.Entities.Offer;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.core.Entities.Branchs;
using Ramz_Elktear.core.Entities.Booking;

namespace Ramz_Elktear.core
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Parameterless constructor for design-time
        public ApplicationDbContext()
        {
        }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Paths> Paths { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<CarCategory> CarCategory { get; set; }
        public virtual DbSet<CarColor> CarColor { get; set; }
        public virtual DbSet<CarModel> CarModel { get; set; }
        public virtual DbSet<ImageCar> ImageCars { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                   "Data Source=SQL9001.site4now.net;Initial Catalog=db_aa1665_ox;User Id=db_aa1665_ox_admin;Password=Chanks@100");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "dbo");
            modelBuilder.Entity<ApplicationRole>().ToTable("Role", "dbo");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole", "dbo");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "dbo");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "dbo");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "dbo");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "dbo");
        }
    }
}
