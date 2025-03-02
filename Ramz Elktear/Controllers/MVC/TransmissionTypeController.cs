using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.TransmissionTypeModels;
using System;
using System.Threading.Tasks;

namespace Ramz_Elktear.Web.Controllers
{
    public class TransmissionTypeController : Controller
    {
        private readonly ITransmissionTypeService _transmissionTypeService;

        public TransmissionTypeController(ITransmissionTypeService transmissionTypeService)
        {
            _transmissionTypeService = transmissionTypeService;
        }

        // GET: TransmissionType
        public async Task<IActionResult> Index()
        {
            var transmissionTypes = await _transmissionTypeService.GetAllTransmissionTypesAsync();
            return View(transmissionTypes);
        }

        // GET: TransmissionType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TransmissionType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddTransmissionType transmissionTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(transmissionTypeDto);
            }

            try
            {
                await _transmissionTypeService.AddTransmissionTypeAsync(transmissionTypeDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(transmissionTypeDto);
            }
        }

        // GET: TransmissionType/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var transmissionType = await _transmissionTypeService.GetTransmissionTypeByIdAsync(id);
            if (transmissionType == null)
            {
                return NotFound();
            }

            var addTransmissionTypeDto = new UpdateTransmissionType
            {
                Id = id,
                NameAr = transmissionType.NameAr,
                NameEn = transmissionType.NameEn,
                DescriptionAr = transmissionType.DescriptionAr,
                DescriptionEn = transmissionType.DescriptionEn,
                IsActive = transmissionType.IsActive
            };

            return View(addTransmissionTypeDto);
        }

        // POST: TransmissionType/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTransmissionType transmissionTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return View(transmissionTypeDto);
            }

            try
            {
                var isUpdated = await _transmissionTypeService.UpdateTransmissionTypeAsync(transmissionTypeDto);
                if (isUpdated != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
                    return View(transmissionTypeDto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(transmissionTypeDto);
            }
        }

        // POST: TransmissionType/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                var isDeleted = await _transmissionTypeService.DeleteTransmissionTypeAsync(id);
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
