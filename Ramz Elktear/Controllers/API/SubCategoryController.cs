using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.CategoryModels;

namespace Ramz_Elktear.Controllers.API
{
    public class SubCategoryController : BaseController
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        // Get All SubCategories
        [HttpGet("AllSubCategories")]
        public async Task<IActionResult> GetAllSubCategories()
        {
            try
            {
                var subCategories = await _subCategoryService.GetAllSubCategoriesAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = subCategories,
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

        // Get SubCategory By Id
        [HttpGet("SubCategory")]
        public async Task<IActionResult> GetSubCategoryById([FromQuery] string id)
        {
            try
            {
                var subCategory = await _subCategoryService.GetSubCategoryByIdAsync(id);
                if (subCategory == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "SubCategory not found."
                    };
                    return NotFound(response);
                }

                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = subCategory,
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
