using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using System.Drawing;

namespace Ramz_Elktear.Controllers.API
{
    public class ColorController : BaseController
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        // Get All Brands
        [HttpGet("Colors")]
        public async Task<IActionResult> GetAllColors()
        {
            try
            {
                var colors = await _colorService.GetAllColorsAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = colors,
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

        // Get Brand By Id
        [HttpGet("Color")]
        public async Task<IActionResult> GetColorById([FromQuery] string id)
        {
            try
            {
                var color = await _colorService.GetColorByIdAsync(id);
                if (color == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "Color not found."
                    };
                    return NotFound(response);
                }

                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = color,
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
    }
}
