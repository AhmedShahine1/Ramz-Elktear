using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;

namespace Ramz_Elktear.Controllers.API
{
    public class HomeController : BaseController
    {
        private readonly ICarService _carService;
        private readonly IPromotionService _promotionService;

        public HomeController(ICarService carService, IPromotionService promotionService)
        {
            _carService = carService;
            _promotionService = promotionService;
        }

        [HttpGet("HomePage")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            var promotion = await _promotionService.GetAllPromotionsAsync();
            var response = new BaseResponse
            {
                status = true,
                Data = new
                {
                    Cars = cars,
                    promotion = promotion,
                    Support = "https://rec.sa/",
                    PhoneNumber = "966560256655"
                },
                ErrorCode = 0,
                ErrorMessage = string.Empty
            };
            return Ok(response);
        }

    }
}
