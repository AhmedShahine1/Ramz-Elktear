using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.InstallmentRequestModels;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.Controllers.MVC
{
    public class InstallmentRequestsController : Controller
    {
        private readonly IInstallmentRequestService _installmentRequestService;

        public InstallmentRequestsController(IInstallmentRequestService installmentRequestService)
        {
            _installmentRequestService = installmentRequestService;
        }

        // GET: InstallmentRequests
        public async Task<IActionResult> Index(InstallmentRequestFilter filter)
        {
            var requests = await _installmentRequestService.FilterInstallmentRequests(filter);
            return View(requests);
        }

        // POST: Update Status
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string id, string newStatus)
        {
            try
            {
                if (Enum.TryParse(newStatus, out InstallmentStatus status))
                {
                    await _installmentRequestService.UpdateInstallmentRequestStatus(id, status);
                    TempData["SuccessMessage"] = "Status updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid status selected.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating status: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}

