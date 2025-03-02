using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CategoryModels;
using System;
using System.Threading.Tasks;

namespace Ramz_Elktear.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            try
            {
                await _categoryService.AddCategoryAsync(categoryDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoryDto);
            }
        }

        // GET: Category/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var addCategoryDto = new UpdateCategoryDTO
            {
                Id = id,
                NameAr = category.NameAr,
                NameEn = category.NameEn,
                DescriptionAr = category.DescriptionAr,
                DescriptionEn = category.DescriptionEn,
                IsActive = category.IsActive
            };

            return View(addCategoryDto);
        }

        // POST: Category/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryDTO categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryDto);
            }

            try
            {
                await _categoryService.UpdateCategoryAsync(categoryDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(categoryDto);
            }
        }

        // GET: Category/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
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
