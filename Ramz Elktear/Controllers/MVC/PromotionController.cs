using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.PromotionModels;

namespace Ramz_Elktear.Controllers.MVC
{
    public class PromotionController : Controller
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        public async Task<IActionResult> Index()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            return View(promotions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddPromotion model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _promotionService.AddPromotionAsync(model);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var promotion = await _promotionService.GetPromotionByIdAsync(id);
            if (promotion == null)
                return NotFound();

            return View(promotion);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, AddPromotion model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _promotionService.UpdatePromotionAsync(id, model);
            return RedirectToAction("Index");
        }
    }
}