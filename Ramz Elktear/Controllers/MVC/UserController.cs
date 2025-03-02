using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using System.Threading.Tasks;

namespace Ramz_Elktear.Controllers.MVC
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: /users/sales
        [HttpGet("sales")]
        public async Task<IActionResult> Sales()
        {
            var salesUsers = await _accountService.GetUsersWithSalesReturnRole();
            return View(salesUsers);
        }
    }
}
