using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BookingModels;

namespace Ramz_Elktear.Controllers.MVC
{
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET: /Booking/
        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return View(bookings);
        }

        // GET: /Booking/Details/{id}
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: /Booking/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingDto createBookingDto)
        {
            if (ModelState.IsValid)
            {
                var newBooking = await _bookingService.AddBookingAsync(createBookingDto);
                if (newBooking != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(createBookingDto);
        }
    }

}
