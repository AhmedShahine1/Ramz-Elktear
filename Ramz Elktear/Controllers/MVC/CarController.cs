using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.BrandModels;
using Ramz_Elktear.core.DTO.ColorModels;
using Ramz_Elktear.core.DTO.FuelTypeModels;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.DTO.TransmissionTypeModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;

namespace Ramz_Elktear.Web.Controllers
{
    [Authorize(Policy = "Admin")]
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        private readonly IOfferService _offerService;
        private readonly IColorService _colorService;
        private readonly IBrandService _brandService;
        private readonly ITransmissionTypeService _transmissionTypeService;
        private readonly IFuelTypeService _fuelTypeService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly IOptionService _optionService;
        private readonly IEngineSizeService _engineSizeService;
        private readonly IOriginService _originService;
        private readonly IModelYearService _modelYearService;
        private readonly IEnginePositionService _enginePositionService;
        private readonly ISpecificationService _specificationService;

        public CarController(ICarService carService, IOfferService offerService, IColorService colorService, IBrandService brandService, ITransmissionTypeService transmissionTypeService, IFuelTypeService fuelTypeService, ISubCategoryService subCategoryService, IOptionService optionService, IEngineSizeService engineSizeService, IOriginService originService, IModelYearService modelYearService, IEnginePositionService enginePositionService, ISpecificationService specificationService)
        {
            _carService = carService;
            _offerService = offerService;
            _colorService = colorService;
            _brandService = brandService;
            _transmissionTypeService = transmissionTypeService;
            _fuelTypeService = fuelTypeService;
            _subCategoryService = subCategoryService;
            _optionService = optionService;
            _engineSizeService = engineSizeService;
            _originService = originService;
            _modelYearService = modelYearService;
            _enginePositionService = enginePositionService;
            _specificationService = specificationService;
        }

        // Method to populate dropdown lists
        private async Task PopulateDropdownLists()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            ViewData["Brands"] = brands.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.NameEn
            }).ToList();

            var colors = await _colorService.GetAllColorsAsync();
            ViewData["Colors"] = colors.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            var transmissionTypes = await _transmissionTypeService.GetAllTransmissionTypesAsync();
            ViewData["TransmissionTypes"] = transmissionTypes.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.NameEn
            }).ToList();

            var fuelTypes = await _fuelTypeService.GetAllFuelTypesAsync();
            ViewData["FuelTypes"] = fuelTypes.Select(f => new SelectListItem
            {
                Value = f.Id.ToString(),
                Text = f.NameEn
            }).ToList();

            var engineSizes = await _engineSizeService.GetAllEngineSizesAsync();
            ViewData["EngineSizes"] = engineSizes.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.NameEn
            }).ToList();

            var origins = await _originService.GetAllOriginsAsync();
            ViewData["Origins"] = origins.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.NameEn
            }).ToList();

            var modelYears = await _modelYearService.GetAllModelYearsAsync();
            ViewData["ModelYears"] = modelYears.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToList();

            var enginePositions = await _enginePositionService.GetAllEnginePositionsAsync();
            ViewData["EnginePositions"] = enginePositions.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.NameEn
            }).ToList();

            var specifications = await _specificationService.GetAllSpecificationsAsync();
            ViewData["Specifications"] = specifications.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.NameEn
            }).ToList();

            var offers = await _offerService.GetAllOffersAsync();
            ViewData["Offers"] = offers.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.NameEn
            }).ToList();

            var options = await _optionService.GetAllOptionsAsync();
            ViewData["Options"] = options.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.NameEn
            }).ToList();

            var subCategories = await _subCategoryService.GetAllSubCategoriesAsync();
            ViewData["SubCategories"] = subCategories.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.NameEn
            }).ToList();
        }

        // GET: Car/Index
        public async Task<IActionResult> Index()
        {
            var cars = await _carService.GetAllCarsAsync(); // Assuming GetAllCarsAsync() is implemented in ICarService
            return View(cars);
        }

        // GET: Car/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();
            return View(car);
        }

        private bool IsValidAspectRatio(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            using (var image = Image.FromStream(file.OpenReadStream()))
            {
                double aspectRatio = (double)image.Width / image.Height;
                return Math.Abs(aspectRatio - (16.0 / 9.0)) < 0.01;
            }
        }

        // GET: Car/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownLists();
            return View(new AddCar());
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCar carDto)
        {
            if (carDto.Image != null && !IsValidAspectRatio(carDto.Image))
            {
                ModelState.AddModelError("Image", "The image must have a 16:9 aspect ratio.");
            }

            if (!ModelState.IsValid)
            {
                await PopulateDropdownLists();
                return View(carDto);
            }

            try
            {
                await _carService.AddCarAsync(carDto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Failed to add car. Please try again.");
                await PopulateDropdownLists();
                return View(carDto);
            }
        }

        // GET: Car/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null) return NotFound();

            await PopulateDropdownLists();

            var editCarDto = new UpdateCarDTO
            {
                Id = car.Id,
                NameAr = car.NameAr,
                NameEn = car.NameEn,
                BrandId = car.Brand?.Id,
                TransmissionTypeId = car.TransmissionType?.Id,
                FuelTypeId = car.FuelType?.Id,
                IsSpecial = car.IsSpecial,
                // Add other necessary properties here
            };

            return View(editCarDto);
        }

        // POST: Car/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCarDTO carDto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownLists();
                return View(carDto);
            }

            try
            {
                var updated = await _carService.UpdateCarAsync(carDto);
                if (updated != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Failed to update car.");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Error updating car. Please try again.");
            }

            await PopulateDropdownLists();
            return View(carDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var Deleted = await _carService.DeleteCarAsync(id);
            if (Deleted)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
