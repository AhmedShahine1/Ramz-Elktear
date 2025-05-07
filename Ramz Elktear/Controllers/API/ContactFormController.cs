using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.ContactForms;

namespace Ramz_Elktear.Controllers.API
{
    public class ContactFormController : BaseController
    {
        private readonly IContactFormService _service;

        public ContactFormController(IContactFormService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactForm([FromBody] AddContactForm request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Invalid request data",
                    Data = ModelState
                });
            }

            try
            {
                var contact = await _service.AddContactFormAsync(request);
                return Ok(new BaseResponse { Data = contact });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An error occurred while submitting the contact form.",
                    Data = ex.Message
                });
            }
        }
    }
}
