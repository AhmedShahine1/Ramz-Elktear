using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.OfferModels;

namespace Ramz_Elktear.Web.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
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
    }
}