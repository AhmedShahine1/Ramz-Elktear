using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.SettingModels;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.Controllers.MVC
{
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        // ✅ Display All Settings (GET)
        public async Task<IActionResult> Index()
        {
            var settings = await _settingService.GetAllSettingsAsync();
            return View(settings);
        }

        // ✅ Show Form to Create a New Setting (GET)
        public IActionResult Create()
        {
            ViewBag.SettingTypes = Enum.GetValues(typeof(SettingType)).Cast<SettingType>();
            return View();
        }

        // ✅ Handle Creating a New Setting (POST)
        [HttpPost]
        public async Task<IActionResult> Create(AddSettingDto settingDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SettingTypes = Enum.GetValues(typeof(SettingType)).Cast<SettingType>();
                return View(settingDto);
            }

            await _settingService.AddSettingAsync(settingDto);
            return RedirectToAction(nameof(Index));
        }

        // ✅ Show Form to Edit an Existing Setting (GET)
        public async Task<IActionResult> Edit(string id)
        {
            var setting = await _settingService.GetSettingByIdAsync(id);
            if (setting == null) return NotFound();

            var updateSettingDto = new UpdateSettingDto
            {
                Id = setting.Id,
                ImageType = Enum.Parse<SettingType>(setting.ImageType)
            };

            ViewBag.SettingTypes = Enum.GetValues(typeof(SettingType)).Cast<SettingType>();
            return View(updateSettingDto);
        }

        // ✅ Handle Updating an Existing Setting (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSettingDto settingDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SettingTypes = Enum.GetValues(typeof(SettingType)).Cast<SettingType>();
                return View(settingDto);
            }

            var result = await _settingService.UpdateSettingAsync(settingDto);
            if (!result) return BadRequest("Update failed");

            return RedirectToAction(nameof(Index));
        }
    }
}