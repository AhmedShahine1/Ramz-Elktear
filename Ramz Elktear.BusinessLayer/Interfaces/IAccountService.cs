using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.RegisterModels;
using Ramz_Elktear.core.DTO.RoleModels;
using Ramz_Elktear.core.DTO.UpdateModels;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Files;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser> GetUserById(string id);
        Task<List<ApplicationUser>> GetAllUsers();
        Task<List<ApplicationRole>> GetAllRoles();
        Task<List<UserRoleDTO>> GetAllUsersWithRoles();
        Task<List<ApplicationUser>> GetUsersInRole(string roleName);
        Task<bool> AssignManagerToSalesAsync(string salesId, string managerId);
        Task<bool> IsPhoneAsync(string Phone);
        Task<IdentityResult> RegisterAdmin(RegisterAdmin model);
        Task<IdentityResult> RegisterSales(RegisterSales model);
        Task<IdentityResult> UpdateAdmin(string adminId, RegisterAdmin model);
        Task<IdentityResult> RegisterSupportDeveloper(RegisterSupportDeveloper model);
        Task<IdentityResult> Register(RegisterUser model);
        Task<IdentityResult> UpdateSupportDeveloper(string SupportDeveloperId, RegisterSupportDeveloper model);
        Task<IdentityResult> RegisterCustomer(RegisterCustomer model);
        Task<ServiceResult<string>> UpdateUserProfileImage(string userId, IFormFile profileImage);
        Task<IdentityResult> UpdateUserPhoneNumber(string userId, string phoneNumber);
        Task<IdentityResult> ChangeUserPassword(ChangePasswordModel model);
        Task<IdentityResult> UpdateUserDetails(UpdateUserDetails model);
        Task<(bool IsSuccess, string Token, string ErrorMessage)> Login(LoginModel model);
        Task<(bool IsSuccess, string Token, string ErrorMessage)> LoginAdmin(LoginAdmin model);
        Task<bool> Logout(ApplicationUser user);
        Task<bool> SendOTP(string customerEmail);
        Task<bool> ValidateOTP(string customerPhoneNumber, string OTPV);
        Task<ApplicationUser> GetUserFromToken(string token);
        Task<IdentityResult> AssignRolesToUser(RoleUserModel model);
        Task<List<string>> GetRoles();
        Task<string> GetUserProfileImage(string profileId);
        Task<Paths> GetPathByName(string name);
        string ValidateJwtToken(string token);
        int GenerateRandomNo();
        Task<List<AuthDTO>> GetUsersWithSalesReturnRole(string UserId);
        Task<List<AuthDTO>> GetUsersWithSalesReturnRole();
        Task<bool> SetDeviceTokenAsync(string userId, string deviceToken);
        Task<ApplicationUser> GetManagerWithLeastBookingsAsync();
        Task<bool> AssignManagerToSalesAsync(string salesId);
        ////------------------------------------------------------
        Task<IdentityResult> Activate(string userId);
        Task<IdentityResult> Suspend(string userId);

    }
}
