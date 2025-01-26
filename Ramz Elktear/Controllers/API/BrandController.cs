using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.BrandModels;

namespace Ramz_Elktear.Controllers.API
{
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        // Get All Brands
        [HttpGet("AllBrand")]
        public async Task<IActionResult> GetAllBrands()
        {
            try
            {
                var brands = await _brandService.GetAllBrandsAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = brands,
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
        [HttpGet("Brand")]
        public async Task<IActionResult> GetBrandById([FromQuery] string id)
        {
            try
            {
                var brand = await _brandService.GetBrandByIdAsync(id);
                if (brand == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "Brand not found."
                    };
                    return NotFound(response);
                }

                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = brand,
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

        // Create a New Brand
        [HttpPost]
        public async Task<IActionResult> AddBrand([FromForm] AddBrand brandDto)
        {
            try
            {
                if (brandDto == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 400,
                        ErrorMessage = "Invalid data."
                    };
                    return BadRequest(response);
                }

                var newBrand = await _brandService.AddBrandAsync(brandDto);
                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = newBrand,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                };
                return CreatedAtAction(nameof(GetBrandById), new { id = newBrand.Id }, successResponse);
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
