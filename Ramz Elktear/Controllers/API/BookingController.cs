using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.BusinessLayer.Services;
using Ramz_Elktear.Controllers.API;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.BookingModels;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.DTO.RegisterModels;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.API.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IAccountService _accountService;

        public BookingController(IBookingService bookingService, IAccountService accountService)
        {
            _bookingService = bookingService;
            _accountService = accountService;
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPost("RequestCar")]
        public async Task<IActionResult> RequestCar([FromBody] RequestCarDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Invalid model"
                });
            }

            try
            {
                // 🔹 Check if the user exists
                var existingUser = await _accountService.IsPhoneAsync(model.PhoneNumber);

                if (existingUser)
                {
                    // 🔹 User exists, attempt login
                    var loginModel = new LoginModel { PhoneNumber = model.PhoneNumber };
                    var loginResult = await _accountService.Login(loginModel);

                    if (!loginResult.IsSuccess)
                    {
                        return Unauthorized(new BaseResponse
                        {
                            status = false,
                            ErrorCode = 401,
                            ErrorMessage = "Login failed"
                        });
                    }
                    var user = await _accountService.GetUserFromToken(loginResult.Token);

                    // 🔹 Create car request
                    var bookingDto = new CreateBookingDto
                    {
                        CarId = model.CarId,
                        UserId = user.Id
                    };
                    var bookingResult = await _bookingService.AddBookingAsync(bookingDto);

                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = bookingResult
                    });
                }
                else
                {
                    // 🔹 User does not exist, register them
                    var registerModel = new RegisterCustomer
                    {
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber
                    };
                    var registerResult = await _accountService.RegisterCustomer(registerModel);

                    if (!registerResult.Succeeded)
                    {
                        return BadRequest(new BaseResponse
                        {
                            status = false,
                            ErrorCode = 500,
                            ErrorMessage = "User registration failed.",
                            Data = registerResult.Errors.Select(e => e.Description).ToArray()
                        });
                    }

                    // 🔹 Login newly registered user
                    var loginModel = new LoginModel { PhoneNumber = model.PhoneNumber };
                    var loginResult = await _accountService.Login(loginModel);

                    if (!loginResult.IsSuccess)
                    {
                        return Unauthorized(new BaseResponse
                        {
                            status = false,
                            ErrorCode = 401,
                            ErrorMessage = "Login failed after registration"
                        });
                    }
                    var user = await _accountService.GetUserFromToken(loginResult.Token);

                    // 🔹 Create car request
                    var bookingDto = new CreateBookingDto
                    {
                        CarId = model.CarId,
                        UserId = user.Id
                    };
                    var bookingResult = await _bookingService.AddBookingAsync(bookingDto);

                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = bookingResult
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = ex.Message
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

        //[HttpPost("calculate")]
        //public async Task<IActionResult> CalculateInstallment([FromBody] InstallmentCalculationRequest request)
        //{
        //    try
        //    {
        //        var result = await _installmentService.CalculateInstallmentAsync(request);

        //        if (!result.IsApproved)
        //        {
        //            return BadRequest(new BaseResponse
        //            {
        //                status = false,
        //                ErrorCode = 400,
        //                ErrorMessage = "Installment not approved due to high salary percentage or bank rejection.",
        //                Data = null
        //            });
        //        }

        //        return Ok(new BaseResponse
        //        {
        //            status = true,
        //            ErrorCode = 0,
        //            ErrorMessage = null,
        //            Data = result
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new BaseResponse
        //        {
        //            status = false,
        //            ErrorCode = 500,
        //            ErrorMessage = "An error occurred while processing the request.",
        //            Data = ex.Message
        //        });
        //    }
        //}
    }
}