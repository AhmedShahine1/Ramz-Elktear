using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;

namespace Ramz_Elktear.Controllers.API
{
    public class HomeController : BaseController
    {
        private readonly ICarService _carService;
        private readonly IOfferService _offerService;
        private readonly IBrandService _brandService;

        public HomeController(ICarService carService, IOfferService offerService, IBrandService brandService)
        {
            _carService = carService;
            _offerService = offerService;
            _brandService = brandService;
        }

        [HttpGet("HomePage")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            var offers = await _offerService.GetAllOffersAsync();
            var Brands = await _brandService.GetAllBrandsAsync();

            var response = new BaseResponse
            {
                status = true,
                Data = new
                {
                    Cars = cars,
                    Offers = offers,
                    Brands = Brands
                },
                ErrorCode = 0,
                ErrorMessage = string.Empty
            };
            return Ok(response);
        }

    }
}
