using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.FuelTypeModels;

namespace Ramz_Elktear.Web.Controllers
{
    public class FuelTypeController : Controller
    {
        private readonly IFuelTypeService _fuelTypeService;

        public FuelTypeController(IFuelTypeService fuelTypeService)
        {
            _fuelTypeService = fuelTypeService;
        }

        // GET: FuelType
        public async Task<IActionResult> Index()
        {
            var fuelTypes = await _fuelTypeService.GetAllFuelTypesAsync();
            return View(fuelTypes);
        }

        // GET: FuelType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FuelType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddFuelType fuelTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(fuelTypeDto);
            }

            try
            {
                await _fuelTypeService.AddFuelTypeAsync(fuelTypeDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(fuelTypeDto);
            }
        }

        // GET: FuelType/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var fuelType = await _fuelTypeService.GetFuelTypeByIdAsync(id);
            if (fuelType == null)
            {
                return NotFound();
            }

            var addFuelTypeDto = new UpdateFuelType
            {
                Id = id,
                NameAr = fuelType.NameAr,
                NameEn = fuelType.NameEn,
                DescriptionAr = fuelType.DescriptionAr,
                DescriptionEn = fuelType.DescriptionEn,
                IsActive = fuelType.IsActive
            };

            return View(addFuelTypeDto);
        }

        // POST: FuelType/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateFuelType fuelTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(fuelTypeDto);
            }

            try
            {
                await _fuelTypeService.UpdateFuelTypeAsync(fuelTypeDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(fuelTypeDto);
            }
        }

        // GET: FuelType/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var fuelType = await _fuelTypeService.GetFuelTypeByIdAsync(id);
            if (fuelType == null)
            {
                return NotFound();
            }

            return View(fuelType);
        }

        // POST: FuelType/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _fuelTypeService.DeleteFuelTypeAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
