using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.core;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using Ramz_Elktear.RepositoryLayer.Repositories;
using System.Text.Json.Serialization;

namespace Ramz_Elktear.Extensions
{
    public static class ContextServicesExtensions
    {
        public static IServiceCollection AddContextServices(this IServiceCollection services, IConfiguration config)
        {

            //- context && json services
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));//,b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)).UseLazyLoadingProxies());
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddControllersWithViews();
            // IBaseRepository && IUnitOfWork Service
            //services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // only Repository
            services.AddTransient<IUnitOfWork, UnitOfWork>(); // Repository and UnitOfWork

            return services;
        }

    }

}
