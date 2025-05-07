using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.InstallmentRequestModels;
using System.ComponentModel.DataAnnotations;

namespace Ramz_Elktear.Controllers.API
{
    public class InstallmentRequestController : BaseController
    {
        private readonly IInstallmentRequestService _installmentRequestService;
        private readonly IInsurancePercentageService _insurancePercentageService;

        public InstallmentRequestController(IInstallmentRequestService installmentRequestService, IInsurancePercentageService insurancePercentageService)
        {
            _installmentRequestService = installmentRequestService;
            _insurancePercentageService = insurancePercentageService;
        }

        [HttpGet("InsurancePercentages")]
        public async Task<IActionResult> GetAllInsurancePercentages()
        {
            try
            {
                var data = await _insurancePercentageService.GetAllAsync();
                return Ok(new BaseResponse { status = true, Data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred: " + ex.Message
                });
            }
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> Filter([FromQuery] InstallmentRequestFilter filter)
        {
            try
            {
                var result = await _installmentRequestService.FilterInstallmentRequests(filter);
                return Ok(new BaseResponse { status = true, Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse { status = false, ErrorCode = 500, ErrorMessage = ex.Message });
            }
        }

        [HttpGet("DataInstallmentRequest")]
        public async Task<IActionResult> DataInstallmentRequest()
        {
            try
            {
                var result = await _installmentRequestService.DataInstallmentRequest();
                return Ok(new BaseResponse { status = true, Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse { status = false, ErrorCode = 500, ErrorMessage = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddInstallmentRequest model)
        {
            if (!ModelState.IsValid)  // Check if the model is valid
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Validation failed.",
                    Data = ModelState.Values.SelectMany(v => v.Errors)
                                                          .Select(e => e.ErrorMessage)
                                                          .ToList()
                });
            }

            try
            {
                var result = await _installmentRequestService.AddInstallmentRequestWithUserCheck(model);
                return Ok(new BaseResponse { status = true, Data = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new BaseResponse { status = false, ErrorCode = 400, ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse { status = false, ErrorCode = 500, ErrorMessage = ex.Message });
            }
        }

        [HttpPost("Checked")]
        public async Task<IActionResult> Checked([FromBody] AddInstallmentRequest model)
        {
            if (!ModelState.IsValid)  // Check if the model is valid
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Validation failed.",
                    Data = ModelState.Values.SelectMany(v => v.Errors)
                                                          .Select(e => e.ErrorMessage)
                                                          .ToList()
                });
            }

            try
            {
                var result = await _installmentRequestService.CheckInstallmentWithoutRequest(model);
                return Ok(new BaseResponse { status = true, Data = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new BaseResponse { status = false, ErrorCode = 400, ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse { status = false, ErrorCode = 500, ErrorMessage = ex.Message });
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateInstallmentRequest model)
        {
            try
            {
                if (model.Id != model.Id)
                    return BadRequest(new BaseResponse { status = false, ErrorCode = 400, ErrorMessage = "Installment request not found." });

                var result = await _installmentRequestService.UpdateInstallmentRequest(model);
                return Ok(new BaseResponse { status = true, Data = result });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new BaseResponse { status = false, ErrorCode = 400, ErrorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse { status = false, ErrorCode = 500, ErrorMessage = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var success = await _installmentRequestService.DeleteInstallmentRequest(id);
                if (!success)
                    return BadRequest(new BaseResponse { status = false, ErrorCode = 400, ErrorMessage = "Installment request not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse { status = false, ErrorCode = 500, ErrorMessage = ex.Message });
            }
        }
    }
}