using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Ramz_Elktear.core;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Helper;
using System.Text;

namespace Ramz_Elktear.Extensions
{
    public static class IdentityServicesExtensions
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            services.Configure<Jwt>(config.GetSection("JWT"));

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = config["JWT:Issuer"],
                        ValidAudience = config["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin", "Support Developer"));
                options.AddPolicy("Support Developer", policy => policy.RequireRole("Support Developer"));
                options.AddPolicy("Customer", policy => policy.RequireRole("Customer", "Admin", "Support Developer"));
                options.AddPolicy("Sales", policy => policy.RequireRole("Sales", "Admin", "Support Developer"));
            });

            return services;
        }
    }

}
