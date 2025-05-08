using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.LocalizationModel;
using System.Security.Claims;

namespace Ramz_Elktear.Controllers.MVC
{
    [Authorize(Roles = "Admin")]
    public class LocalizationManagerController : Controller
    {
        private readonly ILocalizationService _localizationService;

        public LocalizationManagerController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        // GET: LocalizationManager
        public async Task<IActionResult> Index(string searchString, string group, int page = 1)
        {
            var viewModel = await _localizationService.GetLocalizationResourcesAsync(searchString, group, page);
            return View(viewModel);
        }

        // GET: LocalizationManager/GetEditModel/5
        [HttpGet]
        public async Task<IActionResult> GetEditModel(int id)
        {
            var model = await _localizationService.GetLocalizationResourceByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return Json(model);
        }

        // POST: LocalizationManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LocalizationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
                var result = await _localizationService.CreateLocalizationResourceAsync(model, userId);

                if (result)
                {
                    TempData["SuccessMessage"] = "Resource created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = $"A resource with key '{model.Resource.ResourceKey}' already exists.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create resource. Please check input data.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: LocalizationManager/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LocalizationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
                var result = await _localizationService.UpdateLocalizationResourceAsync(model, userId);

                if (result)
                {
                    TempData["SuccessMessage"] = "Resource updated successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Resource not found or could not be updated.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update resource. Please check input data.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: LocalizationManager/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _localizationService.DeleteLocalizationResourceAsync(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Resource deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Resource not found or could not be deleted.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: LocalizationManager/RefreshCache
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RefreshCache()
        {
            _localizationService.RefreshLocalizationCache();
            TempData["SuccessMessage"] = "Localization cache has been successfully refreshed.";
            return RedirectToAction(nameof(Index));
        }

        // POST: LocalizationManager/Import
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "No file was uploaded.";
                return RedirectToAction(nameof(Index));
            }

            if (file.Length > 1024 * 1024 * 5) // 5MB limit
            {
                TempData["ErrorMessage"] = "File size exceeds 5MB limit.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                bool result = false;

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    var content = await reader.ReadToEndAsync();

                    if (fileExtension == ".json")
                    {
                        result = await _localizationService.ImportJsonDataAsync(content, userId);
                    }
                    else if (fileExtension == ".csv")
                    {
                        result = await _localizationService.ImportCsvDataAsync(content, userId);
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unsupported file format. Please upload JSON or CSV file.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                if (result)
                {
                    TempData["SuccessMessage"] = "Localization data has been successfully imported.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to import localization data.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error importing localization data: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LocalizationManager/Export
        public async Task<IActionResult> Export(LocalizationExportRequest request)
        {
            if (string.IsNullOrEmpty(request.Format) || (request.Format.ToLower() != "json" && request.Format.ToLower() != "csv"))
            {
                request.Format = "json";
            }

            try
            {
                var (fileContents, contentType, fileName) = await _localizationService.ExportResourcesAsync(request.Format);

                if (fileContents == null || fileContents.Length == 0)
                {
                    TempData["ErrorMessage"] = "No data to export.";
                    return RedirectToAction(nameof(Index));
                }

                return File(fileContents, contentType, fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error exporting localization data: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: LocalizationManager/ScanMissingKeys
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScanMissingKeys()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "System";
                var addedCount = await _localizationService.ScanMissingKeysAsync(userId);

                if (addedCount > 0)
                {
                    TempData["SuccessMessage"] = $"Added {addedCount} missing localization keys.";
                }
                else
                {
                    TempData["InfoMessage"] = "No missing localization keys found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error scanning for missing keys: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}