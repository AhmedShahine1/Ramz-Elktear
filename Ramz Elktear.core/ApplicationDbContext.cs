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
using Ramz_Elktear.core.Entities.Color;
using Ramz_Elktear.core.Entities.Categories;
using Ramz_Elktear.core.Entities.Specificate;
using Ramz_Elktear.core.Entities.Promotion;
using Ramz_Elktear.core.Entities;

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
        public DbSet<Car> Cars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<TransmissionType> TransmissionTypes { get; set; }
        public DbSet<FuelType> FuelTypes { get; set; }
        public DbSet<EngineSize> EngineSizes { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<ModelYear> ModelYears { get; set; }
        public DbSet<EnginePosition> EnginePositions { get; set; }
        public DbSet<Colors> Colors { get; set; }
        public DbSet<CarColor> CarColors { get; set; }
        public DbSet<ImageCar> CarImages { get; set; }
        public DbSet<CarOffer> CarOffers { get; set; }
        public DbSet<CarSpecification> CarSpecifications { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        //----------------------------------------------------------------------------------
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<InsurancePercentage> InsurancePercentages { get; set; }
        public virtual DbSet<InstallmentRequest> InstallmentRequests { get; set; }
        public virtual DbSet<ContactForm> ContactForms { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                   "Data Source=SQL9001.site4now.net;Initial Catalog=db_aa1665_ramzalkhtear;User Id=db_aa1665_ramzalkhtear_admin;Password=RamzForCar@1#2025;");
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
            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.ImageAr)
                .WithMany()
                .HasForeignKey(p => p.ImageArId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.ImageEn)
                .WithMany()
                .HasForeignKey(p => p.ImageEnId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Brand)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on Brand

            modelBuilder.Entity<Car>()
                .HasOne(c => c.SubCategory)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Set SubCategory to null on delete

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Image)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Set Image to null on delete

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Option)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on Option

            modelBuilder.Entity<Car>()
                .HasOne(c => c.TransmissionType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on TransmissionType

            modelBuilder.Entity<Car>()
                .HasOne(c => c.FuelType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on FuelType

            modelBuilder.Entity<Car>()
                .HasOne(c => c.EngineSize)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on EngineSize

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Origin)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on Origin

            modelBuilder.Entity<Car>()
                .HasOne(c => c.ModelYear)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete on ModelYear

            modelBuilder.Entity<Car>()
                .HasOne(c => c.EnginePosition)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SubCategory>()
                .HasOne(s => s.Category)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SubCategory>()
                .HasOne(s => s.Brand)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CarColor>()
                .HasKey(cc => new { cc.CarId, cc.ColorId });

            modelBuilder.Entity<CarOffer>()
                .HasKey(co => new { co.CarId, co.OfferId });

            modelBuilder.Entity<CarSpecification>()
                .HasKey(cs => new { cs.carId, cs.SpecificationId });

            // Define relationships
            modelBuilder.Entity<Car>()
                .HasOne(c => c.Brand)
                .WithMany(b => b.Cars)
                .HasForeignKey(c => c.BrandId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.SubCategory)
                .WithMany(sc => sc.Cars)
                .HasForeignKey(c => c.SubCategoryId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Option)
                .WithMany(o => o.Cars)
                .HasForeignKey(c => c.OptionId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.TransmissionType)
                .WithMany(tt => tt.Cars)
                .HasForeignKey(c => c.TransmissionTypeId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.FuelType)
                .WithMany(ft => ft.Cars)
                .HasForeignKey(c => c.FuelTypeId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.EngineSize)
                .WithMany(es => es.Cars)
                .HasForeignKey(c => c.EngineSizeId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Origin)
                .WithMany(o => o.Cars)
                .HasForeignKey(c => c.OriginId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.ModelYear)
                .WithMany(my => my.Cars)
                .HasForeignKey(c => c.ModelYearId);

            modelBuilder.Entity<Car>()
                .HasOne(c => c.EnginePosition)
                .WithMany(ep => ep.Cars)
                .HasForeignKey(c => c.EnginePositionId);
        }
    }
}
