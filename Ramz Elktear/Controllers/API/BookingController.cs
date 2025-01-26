using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.Controllers.API;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.BookingModels;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.API.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // Get All Bookings
        [HttpGet("Books")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingService.GetAllBookingsAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = bookings,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"Internal Server Error: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        // Get Booking By Id
        [HttpGet("Book")]
        public async Task<IActionResult> GetBookingById([FromQuery][Required]string id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "Booking not found"
                    };
                    return NotFound(response);
                }

                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = booking,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"Internal Server Error: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }

        // Add a New Booking
        [HttpPost("Book")]
        public async Task<IActionResult> AddBooking([FromBody] CreateBookingDto createBookingDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 400,
                        ErrorMessage = "Invalid data"
                    };
                    return BadRequest(response);
                }

                var newBooking = await _bookingService.AddBookingAsync(createBookingDto);
                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = newBooking,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return CreatedAtAction(nameof(GetBookingById), new { id = newBooking.Id }, successResponse);
            }
            catch (Exception ex)
            {
                var response = new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"Internal Server Error: {ex.Message}"
                };
                return StatusCode(500, response);
            }
        }
    }
}
