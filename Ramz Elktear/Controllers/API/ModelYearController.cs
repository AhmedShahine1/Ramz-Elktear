using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;

namespace Ramz_Elktear.Controllers.API
{
    public class ModelYearController : BaseController
    {
        private readonly IModelYearService _modelYearService;

        public ModelYearController(IModelYearService modelYearService)
        {
            _modelYearService = modelYearService;
        }

        // Get All Brands
        [HttpGet("AllModelYear")]
        public async Task<IActionResult> GetAllModelYears()
        {
            try
            {
                var modelYears = await _modelYearService.GetAllModelYearsAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = modelYears,
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
        [HttpGet("ModelYear")]
        public async Task<IActionResult> GetModelYearById([FromQuery] string id)
        {
            try
            {
                var modelYear = await _modelYearService.GetModelYearByIdAsync(id);
                if (modelYear == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "modelYear not found."
                    };
                    return NotFound(response);
                }

                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = modelYear,
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
