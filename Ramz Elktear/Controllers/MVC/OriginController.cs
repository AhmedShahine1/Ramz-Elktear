using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.OriginModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Ramz_Elktear.Web.Controllers
{
    public class OriginController : Controller
    {
        private readonly IOriginService _originService;

        public OriginController(IOriginService originService)
        {
            _originService = originService;
        }

        // GET: Origin
        public async Task<IActionResult> Index()
        {
            var origins = await _originService.GetAllOriginsAsync();
            return View(origins);
        }

        // GET: Origin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Origin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddOrigin originDto)
        {
            if (!ModelState.IsValid)
            {
                return View(originDto);
            }

            try
            {
                await _originService.AddOriginAsync(originDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(originDto);
            }
        }

        // GET: Origin/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var origin = await _originService.GetOriginByIdAsync(id);
            if (origin == null)
            {
                return NotFound();
            }

            var addOriginDto = new UpdateOrigin
            {
                Id = id,
                NameAr = origin.NameAr,
                NameEn = origin.NameEn,
                DescriptionAr = origin.DescriptionAr,
                DescriptionEn = origin.DescriptionEn,
                IsActive = origin.IsActive
            };

            return View(addOriginDto);
        }

        // POST: Origin/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateOrigin originDto)
        {
            if (!ModelState.IsValid)
            {
                return View(originDto);
            }

            try
            {
                var isUpdated = await _originService.UpdateOriginAsync(originDto);
                if (isUpdated != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
                    return View(originDto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(originDto);
            }
        }

        // GET: Origin/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var origin = await _originService.GetOriginByIdAsync(id);
            if (origin == null)
            {
                return NotFound();
            }

            return View(origin);
        }

        // POST: Origin/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var isDeleted = await _originService.DeleteOriginAsync(id);
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
