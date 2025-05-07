using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.Entities.ApplicationData;
using System.Security.Claims;

namespace Ramz_Elktear.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IDashboardService dashboardService, UserManager<ApplicationUser> userManager)
        {
            _dashboardService = dashboardService;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Manager"))
            {
                return RedirectToAction("ManagerDashboard");
            }
            else if (roles.Contains("Sales"))
            {
                return RedirectToAction("SalesDashboard");
            }

            var Data = await _dashboardService.GetDashboardDataAsync();
            return View(Data);
        }

        [Authorize(Policy = "Manager")]
        public async Task<IActionResult> ManagerDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = await _dashboardService.GetManagerDashboardData(userId);
            return View("ManagerDashboard", data);
        }

        [Authorize(Policy = "Sales")]
        public async Task<IActionResult> SalesDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = await _dashboardService.GetSalesDashboardData(userId);
            return View("SalesDashboard", data);
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
