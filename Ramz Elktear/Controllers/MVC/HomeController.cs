using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.DTO.HomeModels;

namespace Ramz_Elktear.Controllers.MVC
{
    [Authorize(Policy = "Admin")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var Data = await _dashboardService.GetDashboardDataAsync();


            return View(Data);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
