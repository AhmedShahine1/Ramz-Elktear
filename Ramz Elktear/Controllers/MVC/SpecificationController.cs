using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.SpecificationModels;
using System;
using System.Threading.Tasks;

namespace Ramz_Elktear.Web.Controllers
{
    public class SpecificationController : Controller
    {
        private readonly ISpecificationService _specificationService;

        public SpecificationController(ISpecificationService specificationService)
        {
            _specificationService = specificationService;
        }

        // GET: Specification
        public async Task<IActionResult> Index()
        {
            var specifications = await _specificationService.GetAllSpecificationsAsync();
            return View(specifications);
        }

        // GET: Specification/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Specification/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddSpecification specificationDto)
        {
            if (!ModelState.IsValid)
            {
                return View(specificationDto);
            }

            try
            {
                await _specificationService.AddSpecificationAsync(specificationDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(specificationDto);
            }
        }

        // GET: Specification/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var specification = await _specificationService.GetSpecificationByIdAsync(id);
            if (specification == null)
            {
                return NotFound();
            }

            var addSpecificationDto = new UpdateSpecification
            {
                Id = id,
                NameAr = specification.NameAr,
                NameEn = specification.NameEn,
                IsActive = specification.IsActive
            };

            return View(addSpecificationDto);
        }

        // POST: Specification/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateSpecification specificationDto)
        {
            if (!ModelState.IsValid)
            {
                return View(specificationDto);
            }

            try
            {
                var isUpdated = await _specificationService.UpdateSpecificationAsync(specificationDto);
                if (isUpdated != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
                    return View(specificationDto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(specificationDto);
            }
        }

        // GET: Specification/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var specification = await _specificationService.GetSpecificationByIdAsync(id);
            if (specification == null)
            {
                return NotFound();
            }

            return View(specification);
        }

        // POST: Specification/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var isDeleted = await _specificationService.DeleteSpecificationAsync(id);
                if (isDeleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Delete failed. Please try again.");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
