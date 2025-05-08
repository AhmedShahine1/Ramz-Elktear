using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Ramz_Elktear.BusinessLayer.Localization;
using System.Globalization;

namespace Ramz_Elktear.Extensions
{
    public static class LocalizationExtensions
    {
        public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        {
            // Add localization services with path to resource files (as fallback)
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Replace the default string localizer with our database-based implementation
            services.AddSingleton<IStringLocalizerFactory, DatabaseStringLocalizerFactory>();
            services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

            // Configure supported cultures
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                // Clear the default providers and add our custom ones
                options.RequestCultureProviders.Clear();

                // Add our custom cookie provider first - it uses the cookie value directly as culture
                options.RequestCultureProviders.Add(new CustomCookieRequestCultureProvider("userLanguage"));

                // Add standard ASP.NET Core cookie provider as fallback
                options.RequestCultureProviders.Add(new CookieRequestCultureProvider
                {
                    CookieName = CookieRequestCultureProvider.DefaultCookieName
                });

                // Then check query string and Accept-Language header as further fallbacks
                options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });

            return services;
        }

        public static IApplicationBuilder UseLocalizationMiddleware(this IApplicationBuilder app)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            // Apply localization early in the pipeline
            app.UseRequestLocalization(locOptions.Value);

            return app;
        }

        public class CustomCookieRequestCultureProvider : RequestCultureProvider
        {
            private readonly string _cookieName;

            public CustomCookieRequestCultureProvider(string cookieName)
            {
                _cookieName = cookieName;
            }

            /// <summary>
            /// Determines the culture for the current request
            /// </summary>
            public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
            {
                if (httpContext == null)
                {
                    return Task.FromResult((ProviderCultureResult)null);
                }

                var cookie = httpContext.Request.Cookies[_cookieName];

                if (string.IsNullOrEmpty(cookie))
                {
                    return Task.FromResult((ProviderCultureResult)null);
                }

                // Return the cookie value directly as both culture and UI culture
                return Task.FromResult(new ProviderCultureResult(cookie, cookie));
            }
        }
    }
}