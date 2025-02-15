﻿using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.InstallmentModels;

namespace Ramz_Elktear.Controllers.API
{
    public class JobController : BaseController
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        // Add Job
        [HttpPost("AddJob")]
        public async Task<IActionResult> AddJob([FromForm] AddJob jobDto)
        {
            try
            {
                if (jobDto == null)
                {
                    return BadRequest(new BaseResponse
                    {
                        status = false,
                        Data = null,
                        ErrorCode = 400,
                        ErrorMessage = "Invalid data."
                    });
                }

                var result = await _jobService.AddJobAsync(jobDto);
                return Ok(new BaseResponse
                {
                    status = true,
                    Data = result,
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

        // Get All Jobs
        [HttpGet("GetJobs")]
        public async Task<IActionResult> GetAllJobs()
        {
            try
            {
                var result = await _jobService.GetAllJobsAsync();
                return Ok(new BaseResponse
                {
                    status = true,
                    Data = result,
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
    }
}
