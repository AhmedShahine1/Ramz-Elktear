using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.EngineSizeModels;
using System;
using System.Threading.Tasks;

namespace Ramz_Elktear.Web.Controllers
{
    public class EngineSizeController : Controller
    {
        private readonly IEngineSizeService _engineSizeService;

        public EngineSizeController(IEngineSizeService engineSizeService)
        {
            _engineSizeService = engineSizeService;
        }

        // GET: EngineSize
        public async Task<IActionResult> Index()
        {
            var engineSizes = await _engineSizeService.GetAllEngineSizesAsync();
            return View(engineSizes);
        }

        // GET: EngineSize/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EngineSize/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddEngineSize engineSizeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(engineSizeDto);
            }

            try
            {
                await _engineSizeService.AddEngineSizeAsync(engineSizeDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(engineSizeDto);
            }
        }

        // GET: EngineSize/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var engineSize = await _engineSizeService.GetEngineSizeByIdAsync(id);
            if (engineSize == null)
            {
                return NotFound();
            }

            var addEngineSizeDto = new UpdateEngineSize
            {
                Id = id,
                NameAr = engineSize.NameAr,
                NameEn = engineSize.NameEn,
                DescriptionAr = engineSize.DescriptionAr,
                DescriptionEn = engineSize.DescriptionEn,
                IsActive = engineSize.IsActive
            };

            return View(addEngineSizeDto);
        }

        // POST: EngineSize/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEngineSize engineSizeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(engineSizeDto);
            }

            try
            {
                await _engineSizeService.UpdateEngineSizeAsync(engineSizeDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(engineSizeDto);
            }
        }

        // POST: EngineSize/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _engineSizeService.DeleteEngineSizeAsync(id);
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
