using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.NotificationModels;
using Ramz_Elktear.core.DTO.RegisterModels;

namespace Ramz_Elktear.Controllers.API
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet("Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomerDetails()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var customer = await _accountService.GetUserFromToken(token);
                if (customer == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 404,
                        ErrorMessage = "Customer not found"
                    });
                }

                var authDto = _mapper.Map<AuthDTO>(customer);

                return Ok(new BaseResponse
                {
                    status = true,
                    Data = authDto
                });
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

        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomer([FromForm] RegisterCustomer model)
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
                var result = await _accountService.RegisterCustomer(model);

                if (result.Succeeded)
                {
                    var modelR = new LoginModel
                    {
                        PhoneNumber = model.PhoneNumber,
                    };

                    var resultSendOTP = await _accountService.SendOTP(model.PhoneNumber);
                    if (resultSendOTP)
                        return Ok(new BaseResponse
                        {
                            status = true,
                            Data= "Succes Register"
                        });
                    return Unauthorized(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 401,
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "User registration failed.",
                    Data = result.Errors.Select(e => e.Description).ToArray()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
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

            var result = await _accountService.Login(model);

            if (result.IsSuccess)
            {
                var user = await _accountService.GetUserFromToken(result.Token);
                var authDto = _mapper.Map<AuthDTO>(user);
                authDto.Token = result.Token;
                return Ok(new BaseResponse
                {
                    status = true,
                    ErrorCode = 200,
                    Data = authDto
                });
            }
            if (result.Token == "405")
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 405,
                    ErrorMessage = result.ErrorMessage
                });

            }
            return Unauthorized(new BaseResponse
            {
                status = false,
                ErrorCode = 401,
                ErrorMessage = result.ErrorMessage
            });
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var user = await _accountService.GetUserFromToken(token);
                var isSuccess = await _accountService.Logout(user);

                if (isSuccess)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "Successfully logged out"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Logout failed"
                });
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

        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ConfirmPhoneNumber([FromQuery] string customerPhoneNumber, [FromQuery] string OTP)
        {
            if (string.IsNullOrEmpty(customerPhoneNumber))
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Customer Phone Number cannot be null or empty"
                });
            }

            try
            {
                var result = await _accountService.ValidateOTP(customerPhoneNumber, OTP);

                if (result)
                {
                    var model = new LoginModel
                    {
                        PhoneNumber = customerPhoneNumber,
                    };
                    var resultLogin = await _accountService.Login(model);

                    if (resultLogin.IsSuccess)
                    {
                        var user = await _accountService.GetUserFromToken(resultLogin.Token);
                        var authDto = _mapper.Map<AuthDTO>(user);
                        authDto.Token = resultLogin.Token;

                        return Ok(new BaseResponse
                        {
                            status = true,
                            Data = authDto
                        });
                    }

                    return Unauthorized(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 401,
                        ErrorMessage = resultLogin.ErrorMessage
                    });

                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "الكود الذي ادخلته غير صحيح",
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new BaseResponse
                {
                    status = false,
                    ErrorCode = 404,
                    ErrorMessage = ex.Message
                });
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

        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTP([FromQuery] string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Customer Phone Number cannot be null or empty"
                });
            }

            try
            {
                var result = await _accountService.SendOTP(PhoneNumber);

                if (result)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = new { Message = "OTP send successfully." }
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Failed to send OTP to phone number.",
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new BaseResponse
                {
                    status = false,
                    ErrorCode = 404,
                    ErrorMessage = ex.Message
                });
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

        [HttpPost("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var customer = await _accountService.GetUserFromToken(token);

                if (customer == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 404,
                        ErrorMessage = "Customer not found"
                    });
                }

                var result = await _accountService.Suspend(customer.Id);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "User account deleted successfully."
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Failed to delete account",
                    Data = result.Errors.Select(e => e.Description).ToArray()
                });
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

        [HttpPost("UpdateDeviceToken")]
        public async Task<IActionResult> SetDeviceToken([FromBody] SetDeviceTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.DeviceToken))
            {
                return BadRequest("UserId and DeviceToken are required.");
            }

            var success = await _accountService.SetDeviceTokenAsync(request.UserId, request.DeviceToken);
            if (!success)
            {
                return NotFound("User not found.");
            }

            return Ok(new { Message = "Device token updated successfully." });
        }
    }

}
