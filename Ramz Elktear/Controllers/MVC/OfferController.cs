using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.DTO.CarOfferModels;
using Ramz_Elktear.core.DTO;

namespace Ramz_Elktear.Web.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly ICarService _carService;
        private readonly ICarOfferService _carOfferService;

        public OfferController(IOfferService offerService, ICarService carService, ICarOfferService carOfferService)
        {
            _offerService = offerService;
            _carService = carService;
            _carOfferService = carOfferService;
        }

        // GET: Offer
        public async Task<IActionResult> Index()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return View(offers);
        }

        // GET: Offer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Offer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddOffer offerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(offerDto);
            }

            try
            {
                await _offerService.AddOfferAsync(offerDto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(offerDto);
            }
        }

        // GET: Offer/Edit/{id}
        public async Task<IActionResult> Edit(string offerId)
        {
            var offer = await _offerService.GetOfferByIdAsync(offerId);
            if (offer == null)
            {
                return NotFound();
            }

            var addOfferDto = new UpdateOffer
            {
                Id = offerId,
                NameAr = offer.NameAr,
                NameEn = offer.NameEn,
                NewPrice = offer.NewPrice,
                StartDate = offer.StartDate,
                EndDate = offer.EndDate,
                Delivery = offer.Delivery
            };

            return View(addOfferDto);
        }

        // POST: Offer/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateOffer offerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(offerDto);
            }

            try
            {
                bool isUpdated = await _offerService.UpdateOfferAsync(offerDto);
                if (isUpdated)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Update failed. Please try again.");
                    return View(offerDto);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(offerDto);
            }
        }

        // GET: Offer/Delete/{id}
        public async Task<IActionResult> Delete(string offerId)
        {
            var offer = await _offerService.GetOfferByIdAsync(offerId);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offer/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string offerId)
        {
            try
            {
                bool isDeleted = await _offerService.DeleteOfferAsync(offerId);
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

        // GET: Get cars for offer assignment (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetCarsForOffer()
        {
            try
            {
                var cars = await _carService.GetAllCarsAsync();
                return Json(new { success = true, data = cars });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Assign offer to car (single car - keeping for backward compatibility)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOfferToCar([FromBody] AddCarOffer carOffer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid data provided." });
                }

                var result = await _carOfferService.AddCarOfferAsync(carOffer);
                if (result != null)
                {
                    return Json(new { success = true, message = "Offer assigned to car successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to assign offer to car." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Assign offer to multiple cars
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOfferToMultipleCars([FromBody] MultipleCarOfferRequest request)
        {
            try
            {
                if (request?.CarOffers == null || !request.CarOffers.Any())
                {
                    return Json(new { success = false, message = "No cars selected." });
                }

                var results = new List<string>();
                var successCount = 0;
                var failureCount = 0;

                foreach (var carOffer in request.CarOffers)
                {
                    try
                    {
                        var result = await _carOfferService.AddCarOfferAsync(carOffer);
                        if (result != null)
                        {
                            successCount++;
                            results.Add($"Car ID {carOffer.CarId}: Success");
                        }
                        else
                        {
                            failureCount++;
                            results.Add($"Car ID {carOffer.CarId}: Failed to assign offer");
                        }
                    }
                    catch (Exception ex)
                    {
                        failureCount++;
                        results.Add($"Car ID {carOffer.CarId}: Error - {ex.Message}");
                    }
                }

                var message = $"Operation completed. {successCount} successful, {failureCount} failed.";

                return Json(new
                {
                    success = successCount > 0,
                    message = message,
                    details = results,
                    successCount = successCount,
                    failureCount = failureCount
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}