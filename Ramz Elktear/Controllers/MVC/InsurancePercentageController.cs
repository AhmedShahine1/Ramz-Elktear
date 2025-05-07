using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.InsurancePercentageModels;
using Ramz_Elktear.core.Entities.Installment;

namespace Ramz_Elktear.Controllers.MVC
{
    [Authorize(Policy = "Admin")]
    public class InsurancePercentageController : Controller
    {
        private readonly IInsurancePercentageService _service;

        public InsurancePercentageController(IInsurancePercentageService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(AddInsurancePercentageDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            await _service.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, InsurancePercentageDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var success = await _service.UpdateAsync(dto);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}