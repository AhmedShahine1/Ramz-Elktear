using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;

namespace Ramz_Elktear.Controllers.API
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Get All Brands
        [HttpGet("AllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = categories,
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
        [HttpGet("Category")]
        public async Task<IActionResult> GetCategoryById([FromQuery] string id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    var response = new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 404,
                        ErrorMessage = "Category not found."
                    };
                    return NotFound(response);
                }

                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = category,
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
