using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.InstallmentModels;

namespace Ramz_Elktear.Controllers.API
{
    public class BankController : BaseController
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }

        // Get All Banks
        [HttpGet("GetBanks")]
        public async Task<IActionResult> GetAllBanks()
        {
            try
            {
                var result = await _bankService.GetAllBanksAsync();
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
