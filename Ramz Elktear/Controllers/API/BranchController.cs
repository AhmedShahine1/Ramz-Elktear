using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.BranchModels;

namespace Ramz_Elktear.Controllers.API
{
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        // Add Branch
        [HttpPost("AddBranch")]
        public async Task<IActionResult> AddBranch([FromForm] AddBranch branchDto)
        {
            try
            {
                if (branchDto == null)
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

                var result = await _branchService.AddBranchAsync(branchDto);
                var successResponse = new BaseResponse
                {
                    status = true,
                    Data = result,
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

        // Get All Branches
        [HttpGet("GetBranches")]
        public async Task<IActionResult> GetAllBranches()
        {
            try
            {
                var result = await _branchService.GetAllBranchesAsync();
                var response = new BaseResponse
                {
                    status = true,
                    Data = result,
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
    }
}
