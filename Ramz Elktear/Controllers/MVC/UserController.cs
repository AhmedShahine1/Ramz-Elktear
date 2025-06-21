using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.RoleModels;
using Ramz_Elktear.core.DTO.UpdateModels;
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

        [HttpGet]
        public async Task<IActionResult> UpdateProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required");
            }

            var user = await _accountService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var model = new UpdateUserProfileViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                CurrentProfileImageUrl = await _accountService.GetUserProfileImage(user.ProfileId)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateUserDetails model)
        {
            if (!ModelState.IsValid)
            {
                return View("UpdateProfile", new UpdateUserProfileViewModel
                {
                    UserId = model.UserId,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email
                });
            }

            try
            {
                var result = await _accountService.UpdateUserDetails(model);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Profile updated successfully";
                    return RedirectToAction("UpdateProfile", new { userId = model.UserId });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the profile");
            }

            return View("UpdateProfile", new UpdateUserProfileViewModel
            {
                UserId = model.UserId,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            });
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required");
            }

            var user = await _accountService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var model = new ChangePasswordViewModel
            {
                UserId = userId,
                UserName = user.FullName,
                ChangePasswordModel = new ChangePasswordModel { UserId = userId }
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var user = await _accountService.GetUserById(viewModel.ChangePasswordModel.UserId);
                viewModel.UserName = user?.FullName;
                return View(viewModel);
            }

            try
            {
                var result = await _accountService.ChangeUserPassword(viewModel.ChangePasswordModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("ChangePassword", new { userId = viewModel.ChangePasswordModel.UserId });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            var userInfo = await _accountService.GetUserById(viewModel.ChangePasswordModel.UserId);
            viewModel.UserName = userInfo?.FullName;
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfileImage(string userId, IFormFile profileImage)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User ID is required" });
            }

            if (profileImage == null || profileImage.Length == 0)
            {
                return Json(new { success = false, message = "Please select an image file" });
            }

            try
            {
                var result = await _accountService.UpdateUserProfileImage(userId, profileImage);
                if (result.IsSuccess)
                {
                    var newImageUrl = await _accountService.GetUserProfileImage(result.Data?.ToString());
                    return Json(new { success = true, message = "Profile image updated successfully", imageUrl = newImageUrl });
                }
                else
                {
                    return Json(new { success = false, message = string.Join(", ", result.Errors )});
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while updating the profile image" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePhoneNumber(UpdatePhoneNumberModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid phone number format" });
            }

            try
            {
                var result = await _accountService.UpdateUserPhoneNumber(model.UserId, model.PhoneNumber);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Phone number updated successfully" });
                }
                else
                {
                    return Json(new { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) });
                }
            }
            catch (ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while updating the phone number" });
            }
        }
    }
}
