using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
                return Ok(new BaseResponse
                {
                    status = true,
                    Data = bookings,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"Internal Server Error: {ex.Message}"
                });
            }
        }

        // Get Booking By Id
        [HttpGet("Book")]
        public async Task<IActionResult> GetBookingById([FromQuery][Required] string id)
        {
            try
            {
                var booking = await _bookingService.GetBookingByIdAsync(id);
                if (booking == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "Booking not found"
                    });
                }

                return Ok(new BaseResponse
                {
                    status = true,
                    Data = booking,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"Internal Server Error: {ex.Message}"
                });
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
                    return BadRequest(new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 400,
                        ErrorMessage = "Invalid data"
                    });
                }

                var userId = User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 401,
                        ErrorMessage = "User ID not found in claims"
                    });
                }

                createBookingDto.UserId = userId;
                var newBooking = await _bookingService.AddBookingAsync(createBookingDto);

                return CreatedAtAction(nameof(GetBookingById), new { id = newBooking.Id }, new BaseResponse
                {
                    status = true,
                    Data = newBooking,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"Internal Server Error: {ex.Message}"
                });
            }
        }

        // Get Bookings By Buyer
        [HttpGet("Booking")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetBookingsByBuyer()
        {
            try
            {
                var buyerId = User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
                var bookings = await _bookingService.GetBookingsByBuyerIdAsync(buyerId);

                return Ok(new BaseResponse
                {
                    status = true,
                    Data = bookings,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"Internal Server Error: {ex.Message}"
                });
            }
        }

        // Delete Booking
        [HttpDelete("Book")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteBooking([FromQuery][Required] string id)
        {
            try
            {
                var result = await _bookingService.DeleteBookingAsync(id);
                if (!result)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "Booking not found"
                    });
                }

                return Ok(new BaseResponse
                {
                    status = true,
                    Data = null,
                    ErrorCode = 0,
                    ErrorMessage = "Booking deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    Data = null,
                    ErrorCode = 500,
                    ErrorMessage = $"Internal Server Error: {ex.Message}"
                });
            }
        }
    }
}
