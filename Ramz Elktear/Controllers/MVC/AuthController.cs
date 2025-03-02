using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.RegisterModels;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.Entities.ApplicationData;
using Microsoft.AspNetCore.Authorization;

namespace Ramz_Elktear.Controllers.MVC
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        [Authorize(Policy = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAdmin model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.RegisterAdmin(model);
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
        
        [Authorize(Policy = "Admin")]
        public IActionResult RegisterSales()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterSales(RegisterSales model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.RegisterSales(model);
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

        public IActionResult Login()
        {
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
                // Get the user details
                var user = await _accountService.GetUserFromToken(result.Token);
                var profileImage = await _accountService.GetUserProfileImage(user.ProfileId);

                // Set user details in cookies
                Response.Cookies.Append("userName", user.UserName, new CookieOptions { HttpOnly = true, Secure = true, MaxAge = TimeSpan.FromDays(7) });
                Response.Cookies.Append("userProfileImage", profileImage, new CookieOptions { HttpOnly = true, Secure = true, MaxAge = TimeSpan.FromDays(7) });

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