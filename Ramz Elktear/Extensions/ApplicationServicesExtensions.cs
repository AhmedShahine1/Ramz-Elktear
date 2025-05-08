using Microsoft.Extensions.Localization;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.BusinessLayer.Localization;
using Ramz_Elktear.BusinessLayer.Mapping;
using Ramz_Elktear.BusinessLayer.Services;

namespace Ramz_Elktear.Extensions
{

    public static class ApplicationServicesExtensions
    {
        // interfaces sevices [IAccountService, IPhotoHandling  ]
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDistributedMemoryCache(); // Add this line to configure the distributed cache

            // Session Service
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(12);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IFileHandling, FileHandling>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<ICarService, CarService>();
            services.AddTransient<IOfferService, OfferService>();
            services.AddTransient<IBankService, BankService>();
            services.AddTransient<IBranchService, BranchService>();
            services.AddTransient<IJobService, JobService>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<ICarColorService, CarColorService>();
            services.AddTransient<ICarOfferService, CarOfferService>();
            services.AddTransient<ICarSpecificationService, CarSpecificationService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<IEnginePositionService, EnginePositionService>();
            services.AddTransient<IEngineSizeService, EngineSizeService>();
            services.AddTransient<IFuelTypeService, FuelTypeService>();
            services.AddTransient<IImageCarService, ImageCarService>();
            services.AddTransient<IModelYearService, ModelYearService>();
            services.AddTransient<IOptionService, OptionService>();
            services.AddTransient<IOriginService, OriginService>();
            services.AddTransient<ISpecificationService, SpecificationService>();
            services.AddTransient<ISubCategoryService, SubCategoryService>();
            services.AddTransient<ITransmissionTypeService, TransmissionTypeService>();
            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<IFirebaseNotificationService, FirebaseNotificationService>();
            services.AddTransient<IPromotionService, PromotionService>();
            services.AddTransient<IInstallmentRequestService, InstallmentRequestService>();
            services.AddTransient<IInsurancePercentageService, InsurancePercentageService>();
            services.AddTransient<IContactFormService, ContactFormService>();
            services.AddTransient<ISettingService, SettingService>();
            services.AddTransient<ILocalizationService, LocalizationService>();

            services.AddHttpClient();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IApplicationBuilder UseApplicationMiddleware(this IApplicationBuilder app)
        {
            app.UseSession();
            /*   app.UseHangfireDashboard("/Hangfire/Dashboard");

               app.UseWebSockets();*/

            return app;
        }
    }

}
