using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.CategoryModels;

namespace Ramz_Elktear.Web.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryService _subCategoryService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public SubCategoryController(ISubCategoryService subCategoryService, IBrandService brandService, ICategoryService categoryService)
        {
            _subCategoryService = subCategoryService;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        // GET: SubCategory
        public async Task<IActionResult> Index()
        {
            var subCategories = await _subCategoryService.GetAllSubCategoriesAsync();
            return View(subCategories);
        }

        // GET: SubCategory/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllCategoriesAsync() ?? new List<CategoryDTO>();
            var brands = await _brandService.GetAllBrandsAsync() ?? new List<BrandDetails>();

            ViewData["Categories"] = categories.Any()
                ? categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NameAr }).ToList()
                : new List<SelectListItem> { new SelectListItem { Text = "No categories available", Value = "" } };

            ViewData["Brands"] = brands.Any()
                ? brands.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.NameAr }).ToList()
                : new List<SelectListItem> { new SelectListItem { Text = "No brands available", Value = "" } };

            return View();
        }

        // POST: SubCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSubCategoryDTO subCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync() ?? new List<CategoryDTO>();
                var brands = await _brandService.GetAllBrandsAsync() ?? new List<BrandDetails>();

                ViewData["Categories"] = categories.Any()
                    ? categories.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NameAr }).ToList()
                    : new List<SelectListItem> { new SelectListItem { Text = "No categories available", Value = "" } };

                ViewData["Brands"] = brands.Any()
                    ? brands.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.NameAr }).ToList()
                    : new List<SelectListItem> { new SelectListItem { Text = "No brands available", Value = "" } };

                return View(subCategoryDto);
            }

            try
            {
                await _subCategoryService.AddSubCategoryAsync(subCategoryDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            // Repopulate dropdowns before returning the view
            var categoriesList = await _categoryService.GetAllCategoriesAsync() ?? new List<CategoryDTO>();
            var brandsList = await _brandService.GetAllBrandsAsync() ?? new List<BrandDetails>();

            ViewData["Categories"] = categoriesList.Any()
                ? categoriesList.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.NameAr }).ToList()
                : new List<SelectListItem> { new SelectListItem { Text = "No categories available", Value = "" } };

            ViewData["Brands"] = brandsList.Any()
                ? brandsList.Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.NameAr }).ToList()
                : new List<SelectListItem> { new SelectListItem { Text = "No brands available", Value = "" } };

            return View(subCategoryDto);
        }

        // GET: SubCategory/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var subCategory = await _subCategoryService.GetSubCategoryByIdAsync(id);
            if (subCategory == null)
            {
                return NotFound();
            }

            // Fetch categories and brands
            var categories = await _categoryService.GetAllCategoriesAsync() ?? new List<CategoryDTO>();
            var brands = await _brandService.GetAllBrandsAsync() ?? new List<BrandDetails>();

            // Ensure dropdown lists are not null
            ViewData["Categories"] = categories.Any()
                ? categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.NameAr,
                    Selected = c.Id == subCategory.Category?.Id
                }).ToList()
                : new List<SelectListItem> { new SelectListItem { Text = "No categories available", Value = "" } };

            ViewData["Brands"] = brands.Any()
                ? brands.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.NameAr,
                    Selected = b.Id == subCategory.Brand?.Id
                }).ToList()
                : new List<SelectListItem> { new SelectListItem { Text = "No brands available", Value = "" } };

            var editSubCategoryDto = new UpdateSubCategoryDTO
            {
                Id = subCategory.Id,
                NameAr = subCategory.NameAr,
                NameEn = subCategory.NameEn,
                CategoryId = subCategory.Category?.Id,
                BrandId = subCategory.Brand?.Id,
                IsActive = subCategory.IsActive
            };

            return View(editSubCategoryDto);
        }

        // POST: SubCategory/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateSubCategoryDTO subCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync() ?? new List<CategoryDTO>();
                var brands = await _brandService.GetAllBrandsAsync() ?? new List<BrandDetails>();

                ViewData["Categories"] = categories.Any()
                    ? categories.Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.NameAr,
                        Selected = c.Id == subCategoryDto.CategoryId
                    }).ToList()
                    : new List<SelectListItem> { new SelectListItem { Text = "No categories available", Value = "" } };

                ViewData["Brands"] = brands.Any()
                    ? brands.Select(b => new SelectListItem
                    {
                        Value = b.Id.ToString(),
                        Text = b.NameAr,
                        Selected = b.Id == subCategoryDto.BrandId
                    }).ToList()
                    : new List<SelectListItem> { new SelectListItem { Text = "No brands available", Value = "" } };

                return View(subCategoryDto);
            }

            try
            {
                var isUpdated = await _subCategoryService.UpdateSubCategoryAsync(subCategoryDto);
                if (isUpdated)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            // Repopulate dropdowns on error
            var categoriesList = await _categoryService.GetAllCategoriesAsync() ?? new List<CategoryDTO>();
            var brandsList = await _brandService.GetAllBrandsAsync() ?? new List<BrandDetails>();

            ViewData["Categories"] = categoriesList.Any()
                ? categoriesList.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.NameAr,
                    Selected = c.Id == subCategoryDto.CategoryId
                }).ToList()
                : new List<SelectListItem> { new SelectListItem { Text = "No categories available", Value = "" } };

            ViewData["Brands"] = brandsList.Any()
                ? brandsList.Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.NameAr,
                    Selected = b.Id == subCategoryDto.BrandId
                }).ToList()
                : new List<SelectListItem> { new SelectListItem { Text = "No brands available", Value = "" } };

            return View(subCategoryDto);
        }

        // POST: SubCategory/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var isDeleted = await _subCategoryService.DeleteSubCategoryAsync(id);
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
