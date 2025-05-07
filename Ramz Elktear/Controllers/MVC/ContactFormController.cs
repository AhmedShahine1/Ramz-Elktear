using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;

namespace Ramz_Elktear.Controllers.MVC
{
    public class ContactFormController : Controller
    {
        private readonly IContactFormService _service;

        public ContactFormController(IContactFormService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _service.GetAllContactFormsAsync();
            return View(contacts);
        }
    }
}
