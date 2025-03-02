using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.ModelYearModels;
using System;
using System.Threading.Tasks;

namespace Ramz_Elktear.Web.Controllers
{
    public class ModelYearController : Controller
    {
        private readonly IModelYearService _modelYearService;

        public ModelYearController(IModelYearService modelYearService)
        {
            _modelYearService = modelYearService;
        }

        // GET: ModelYear
        public async Task<IActionResult> Index()
        {
            var modelYears = await _modelYearService.GetAllModelYearsAsync();
            return View(modelYears);
        }

        // GET: ModelYear/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModelYear/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddModelYear modelYearDto)
        {
            if (!ModelState.IsValid)
            {
                return View(modelYearDto);
            }

            try
            {
                await _modelYearService.AddModelYearAsync(modelYearDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(modelYearDto);
            }
        }

        // GET: ModelYear/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var modelYear = await _modelYearService.GetModelYearByIdAsync(id);
            if (modelYear == null)
            {
                return NotFound();
            }

            var updateModelYearDto = new UpdateModelYear
            {
                Id = id,
                Name = modelYear.Name,
                IsActive = modelYear.IsActive
            };

            return View(updateModelYearDto);
        }

        // POST: ModelYear/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateModelYear modelYearDto)
        {
            if (!ModelState.IsValid)
            {
                return View(modelYearDto);
            }

            try
            {
                await _modelYearService.UpdateModelYearAsync( modelYearDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(modelYearDto);
            }
        }

        // GET: ModelYear/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var modelYear = await _modelYearService.GetModelYearByIdAsync(id);
            if (modelYear == null)
            {
                return NotFound();
            }

            return View(modelYear);
        }

        // POST: ModelYear/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _modelYearService.DeleteModelYearAsync(id);
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
