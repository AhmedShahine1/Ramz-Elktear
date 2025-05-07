using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.RoleModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ramz_Elktear.Controllers.MVC
{
    public class UserController : Controller
    {
        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: /users/sales
        public async Task<IActionResult> Sales()
        {
            var salesUsers = await _accountService.GetUsersWithSalesReturnRole();
            return View(salesUsers);
        }

        public async Task<IActionResult> SalesByManager()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in manager ID
            var salesUsers = await _accountService.GetUsersWithSalesReturnRole(managerId);
            return View("Sales", salesUsers); // Reuse the same view
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AssignUserRoles()
        {
            var users = await _accountService.GetAllUsers();
            var roles = await _accountService.GetAllRoles();

            ViewBag.Roles = roles;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserRoles(RoleUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _accountService.AssignRolesToUser(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return RedirectToAction("AssignUserRoles");
        }

        [HttpGet]
        public async Task<IActionResult> AllUsersWithRoles()
        {
            var users = await _accountService.GetAllUsersWithRoles();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AssignSalesToManager()
        {
            var salesUsers = await _accountService.GetUsersInRole("Sales");
            var managers = await _accountService.GetUsersInRole("Manager");

            ViewBag.Managers = managers;
            return View(salesUsers);
        }

        [HttpPost]
        public async Task<IActionResult> AssignSalesToManager(string salesId, string managerId)
        {
            if (string.IsNullOrEmpty(salesId) || string.IsNullOrEmpty(managerId))
                return BadRequest("Sales and Manager must be selected.");

            var success = await _accountService.AssignManagerToSalesAsync(salesId, managerId);
            if (!success)
                return BadRequest("Failed to assign manager.");

            return RedirectToAction("AssignSalesToManager");
        }
    }
}
