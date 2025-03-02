using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;

namespace Ramz_Elktear.Controllers.API
{
    public class PromotionController : BaseController
    {
        private readonly IPromotionService _promotionService;
        private BaseResponse baseResponse;
        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
            baseResponse = new BaseResponse();
        }
        [HttpGet("Promotions")]
        public async Task<IActionResult> Promotions()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            baseResponse.status = true;
            baseResponse.Data = promotions;
            return Ok(baseResponse);
        }
    }
}
