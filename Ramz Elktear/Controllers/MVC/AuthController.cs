using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.RegisterModels;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.Controllers.MVC
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;
        private readonly ISettingService _settingService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAccountService accountService, ISettingService settingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
            _settingService = settingService;
        }

        public async Task<IActionResult> Register()
        {
            var registerImage = await _settingService.GetSettingByTypeAsync(SettingType.Register);
            var logoUrl = Request.Cookies["siteLogo"] ?? "/assets/img/default-logo.png";

            ViewBag.Logo = logoUrl;
            ViewBag.RegisterImage = registerImage?.ImageUrl ?? "/assets/img/default-register.png";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.Register(model);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }

        public async Task<IActionResult> Login()
        {
            var logo = await _settingService.GetSettingByTypeAsync(SettingType.Logo);
            var loginImage = await _settingService.GetSettingByTypeAsync(SettingType.Login);

            var logoUrl = logo?.ImageUrl ?? "/assets/img/default-logo.png";
            var loginImageUrl = loginImage?.ImageUrl ?? "/assets/img/default-login.png";

            // Save Logo in Cookies
            Response.Cookies.Append("siteLogo", logoUrl, new CookieOptions { HttpOnly = true, Secure = true, MaxAge = TimeSpan.FromDays(7) });

            ViewBag.Logo = logoUrl;
            ViewBag.LoginImage = loginImageUrl;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginAdmin model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.LoginAdmin(model);
            if (result.IsSuccess)
            {
                var user = await _accountService.GetUserFromToken(result.Token);
                var roles = await _userManager.GetRolesAsync(user);

                // Set cookies for username and profile image
                var profileImage = await _accountService.GetUserProfileImage(user.ProfileId);
                Response.Cookies.Append("userName", user.UserName, new CookieOptions { HttpOnly = true, Secure = true, MaxAge = TimeSpan.FromDays(7) });
                Response.Cookies.Append("userProfileImage", profileImage, new CookieOptions { HttpOnly = true, Secure = true, MaxAge = TimeSpan.FromDays(7) });
                Response.Cookies.Append("userRole", roles.First(), new CookieOptions { HttpOnly = true, Secure = true, MaxAge = TimeSpan.FromDays(7) });

                // Redirect based on role
                if (roles.Contains("Manager"))
                {
                    return RedirectToAction("ManagerDashboard", "Home");
                }
                else if (roles.Contains("Sales"))
                {
                    return RedirectToAction("SalesDashboard", "Home");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", result.ErrorMessage);
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("userProfileImage");
            Response.Cookies.Delete("userName");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}