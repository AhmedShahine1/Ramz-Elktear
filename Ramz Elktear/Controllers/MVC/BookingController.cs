using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.Controllers.MVC
{
    [Route("booking")]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(IBookingService bookingService, IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _bookingService = bookingService;
            _accountService = accountService;
            _userManager = userManager;
        }

        // GET: /booking/all
        [HttpGet("all")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _bookingService.GetAllBookingsAsync();
            var salesUsers = await _accountService.GetUsersWithSalesReturnRole(user.Id);
            ViewBag.SalesList = salesUsers;
            return View(bookings);
        }


        // GET: /booking/recieve
        [HttpGet("recieve")]
        public async Task<IActionResult> IndexRecieve()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _bookingService.GetBookingBystatusAsync(BookingStatus.recieve);
            var salesUsers = await _accountService.GetUsersWithSalesReturnRole(user.Id);
            ViewBag.SalesList = salesUsers;
            return View("Index", bookings); // Uses same view as Index
        }

        // GET: /booking/send
        [HttpGet("send")]
        public async Task<IActionResult> IndexSend()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _bookingService.GetBookingBystatusAsync(BookingStatus.send);
            var salesUsers = await _accountService.GetUsersWithSalesReturnRole(user.Id);
            ViewBag.SalesList = salesUsers;
            return View("Index", bookings); // Uses same view as Index
        }

        [HttpGet("MyBookings")]
        public async Task<IActionResult> MyBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _bookingService.GetBookingByUserAsync(user.Id);
            var salesUsers = await _accountService.GetUsersWithSalesReturnRole(user.Id);
            ViewBag.SalesList = salesUsers;
            return View("Index", bookings); // Uses same view as Index
        }
        
        [HttpGet("SalesBookings")]
        public async Task<IActionResult> SalesBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            var bookings = await _bookingService.GetBookingsByManagerIdAsync(user.Id);
            bookings.AddRange(await _bookingService.GetBookingByUserAsync(user.Id));
            var salesUsers = await _accountService.GetUsersWithSalesReturnRole(user.Id);
            ViewBag.SalesList = salesUsers;
            return View("Index", bookings); // Uses same view as Index
        }

        // GET: /booking/details/{id}
        [HttpGet("details/{id}")]
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

        // GET: /booking/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("AssignSales")]
        public async Task<IActionResult> AssignSales(string bookingId, string salesId)
        {
            var result = await _bookingService.AssignSalesAsync(bookingId, salesId);
            if (result)
                return Ok();
            return BadRequest();
        }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(string bookingId)
        {
            var result = await _bookingService.UpdateBookingStatusAsync(bookingId);
            if (result)
                return Ok();
            return BadRequest();
        }

        // POST: /booking/create
        [HttpPost("create")]
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
