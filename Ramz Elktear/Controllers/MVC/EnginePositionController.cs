using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.EnginePositionModels;
using System;
using System.Threading.Tasks;

namespace Ramz_Elktear.Web.Controllers
{
    public class EnginePositionController : Controller
    {
        private readonly IEnginePositionService _enginePositionService;

        public EnginePositionController(IEnginePositionService enginePositionService)
        {
            _enginePositionService = enginePositionService;
        }

        // GET: EnginePosition
        public async Task<IActionResult> Index()
        {
            var enginePositions = await _enginePositionService.GetAllEnginePositionsAsync();
            return View(enginePositions);
        }

        // GET: EnginePosition/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EnginePosition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddEnginePosition enginePositionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(enginePositionDto);
            }

            try
            {
                await _enginePositionService.AddEnginePositionAsync(enginePositionDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(enginePositionDto);
            }
        }

        // GET: EnginePosition/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var enginePosition = await _enginePositionService.GetEnginePositionByIdAsync(id);
            if (enginePosition == null)
            {
                return NotFound();
            }

            var addEnginePositionDto = new UpdateEnginePosition
            {
                Id = enginePosition.Id,
                NameAr = enginePosition.NameAr,
                NameEn = enginePosition.NameEn,
                DescriptionAr = enginePosition.DescriptionAr,
                DescriptionEn = enginePosition.DescriptionEn,
                IsActive = enginePosition.IsActive
            };

            return View(addEnginePositionDto);
        }

        // POST: EnginePosition/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEnginePosition enginePositionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(enginePositionDto);
            }

            try
            {
                await _enginePositionService.UpdateEnginePositionAsync(enginePositionDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(enginePositionDto);
            }
        }

        // GET: EnginePosition/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var enginePosition = await _enginePositionService.GetEnginePositionByIdAsync(id);
            if (enginePosition == null)
            {
                return NotFound();
            }

            return View(enginePosition);
        }

        // POST: EnginePosition/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _enginePositionService.DeleteEnginePositionAsync(id);
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
