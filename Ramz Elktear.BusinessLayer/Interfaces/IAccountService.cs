using Microsoft.AspNetCore.Identity;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.RegisterModels;
using Ramz_Elktear.core.DTO.RoleModels;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Files;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser> GetUserById(string id);
        Task<IdentityResult> RegisterAdmin(RegisterAdmin model);
        Task<IdentityResult> UpdateAdmin(string adminId, RegisterAdmin model);
        Task<IdentityResult> RegisterSupportDeveloper(RegisterSupportDeveloper model);
        Task<IdentityResult> UpdateSupportDeveloper(string SupportDeveloperId, RegisterSupportDeveloper model);
        Task<IdentityResult> RegisterCustomer(RegisterCustomer model);
        Task<(bool IsSuccess, string Token, string ErrorMessage)> Login(LoginModel model);
        Task<bool> Logout(ApplicationUser user);
        Task<bool> SendOTP(string customerEmail);
        Task<bool> ValidateOTP(string customerPhoneNumber, string OTPV);
        Task<ApplicationUser> GetUserFromToken(string token);
        Task<string> AddRoleAsync(RoleUserModel model);
        Task<List<string>> GetRoles();
        Task<string> GetUserProfileImage(string profileId);
        Task<Paths> GetPathByName(string name);
        string ValidateJwtToken(string token);
        int GenerateRandomNo();
        ////------------------------------------------------------
        Task<IdentityResult> Activate(string userId);
        Task<IdentityResult> Suspend(string userId);

    }
}
