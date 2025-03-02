using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.OptionModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Ramz_Elktear.Web.Controllers
{
    public class OptionController : Controller
    {
        private readonly IOptionService _optionService;

        public OptionController(IOptionService optionService)
        {
            _optionService = optionService;
        }

        // GET: Option
        public async Task<IActionResult> Index()
        {
            var options = await _optionService.GetAllOptionsAsync();
            return View(options);
        }

        // GET: Option/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Option/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOptionDTO optionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(optionDto);
            }

            try
            {
                await _optionService.AddOptionAsync(optionDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(optionDto);
            }
        }

        // GET: Option/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var option = await _optionService.GetOptionByIdAsync(id);
            if (option == null)
            {
                return NotFound();
            }

            var createOptionDto = new UpdateOptionDTO
            {
                Id = id,
                NameAr = option.NameAr,
                NameEn = option.NameEn,
                DescriptionAr = option.DescriptionAr,
                DescriptionEn = option.DescriptionEn,
                IsActive = option.IsActive
            };

            return View(createOptionDto);
        }

        // POST: Option/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateOptionDTO optionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(optionDto);
            }

            try
            {
                var isUpdated = await _optionService.UpdateOptionAsync(optionDto);
                if (isUpdated != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
                    return View(optionDto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(optionDto);
            }
        }

        // GET: Option/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var option = await _optionService.GetOptionByIdAsync(id);
            if (option == null)
            {
                return NotFound();
            }

            return View(option);
        }

        // POST: Option/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                bool isDeleted = await _optionService.DeleteOptionAsync(id);
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
