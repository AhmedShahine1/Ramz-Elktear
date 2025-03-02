using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BrandModels;

namespace Ramz_Elktear.Web.Controllers
{
    [Authorize(Policy = "Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return View(brands);
        }

        // GET: Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddBrand brandDto)
        {
            if (!ModelState.IsValid)
            {
                return View(brandDto);
            }

            try
            {
                await _brandService.AddBrandAsync(brandDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(brandDto);
            }
        }

        // GET: Brand/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            var updateBrandDto = new UpdateBrandDto
            {
                Id = brand.Id,
                NameAr = brand.NameAr,
                NameEn = brand.NameEn,
                DescriptionAr = brand.DescriptionAr,
                DescriptionEn = brand.DescriptionEn
            };

            return View(updateBrandDto);
        }

        // POST: Brand/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateBrandDto brandDto)
        {
            if (!ModelState.IsValid)
            {
                return View(brandDto);
            }

            try
            {
                await _brandService.UpdateBrandAsync(brandDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(brandDto);
            }
        }

        // POST: Brand/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _brandService.DeleteBrandAsync(id);
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
