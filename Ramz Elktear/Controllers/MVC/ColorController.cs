using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.ColorModels;

namespace Ramz_Elktear.Web.Controllers
{
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        // GET: Color
        public async Task<IActionResult> Index()
        {
            var colors = await _colorService.GetAllColorsAsync();
            return View(colors);
        }

        // GET: Color/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Color/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddColor colorDto)
        {
            if (!ModelState.IsValid)
            {
                return View(colorDto);
            }

            try
            {
                await _colorService.AddColorAsync(colorDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(colorDto);
            }
        }

        // GET: Color/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var color = await _colorService.GetColorByIdAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            var addColorDto = new UpdateColor
            {
                Id = id,
                Name = color.Name,
                IsActive = color.IsActive
            };

            return View(addColorDto);
        }

        // POST: Color/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateColor colorDto)
        {
            if (!ModelState.IsValid)
            {
                return View(colorDto);
            }

            try
            {
                await _colorService.UpdateColorAsync(colorDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(colorDto);
            }
        }

        // GET: Color/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var color = await _colorService.GetColorByIdAsync(id);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        // POST: Color/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _colorService.DeleteColorAsync(id);
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
