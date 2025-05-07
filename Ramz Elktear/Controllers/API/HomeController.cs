using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.PromotionModels;

namespace Ramz_Elktear.Controllers.API
{
    public class HomeController : BaseController
    {
        private readonly ICarService _carService;
        private readonly IPromotionService _promotionService;
        private readonly IBrandService _brandService;
        private readonly IOfferService _offerService;

        public HomeController(ICarService carService, IPromotionService promotionService, IBrandService brandService, IOfferService offerService)
        {
            _carService = carService;
            _promotionService = promotionService;
            _brandService = brandService;
            _offerService = offerService;
        }

        [HttpGet("HomePage")]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            var promotion = (await _promotionService.GetAllPromotionsAsync()).ToList(); // Convert to List
            var brands = await _brandService.GetAllBrandsAsync();
            var offers = await _offerService.GetAllOffersAsync();

            foreach (var offer in offers)
            {
                promotion.Add(new PromotionDetails // Use Add() instead of Append()
                {
                    Id = offer.Id,
                    ImageAr = offer.ImageUrl,
                    ImageEn = offer.ImageUrl,
                    redirctURL = ""
                });
            }
            var response = new BaseResponse
            {
                status = true,
                Data = new
                {
                    Cars = cars,
                    promotion = promotion,
                    brands = brands,
                    Support = "https://rec.sa/",
                    Whatsapp= "00966560256655",
                    PhoneNumber = "8001260333"
                },
                ErrorCode = 0,
                ErrorMessage = string.Empty
            };
            return Ok(response);
        }
    }
}
