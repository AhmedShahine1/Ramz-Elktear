using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Ramz_Elktear.Controllers.MVC
{
    public class LocalizationController : Controller
    {
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (string.IsNullOrEmpty(culture))
            {
                return BadRequest("Culture cannot be null or empty");
            }

            // Set our custom culture cookie
            Response.Cookies.Append(
                "userLanguage",
                culture,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    Path = "/",
                    HttpOnly = false
                }
            );

            // Also set the standard ASP.NET Core culture cookie as fallback
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    Path = "/"
                }
            );

            // Manually update the culture for the current request (for immediate effect)
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            // Reload the page to apply the new culture
            return LocalRedirect(returnUrl ?? "~/");
        }
    }
}