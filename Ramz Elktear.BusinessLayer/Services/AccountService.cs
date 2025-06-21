using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.RegisterModels;
using Ramz_Elktear.core.DTO.RoleModels;
using Ramz_Elktear.core.DTO.UpdateModels;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Helper;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly Jwt _jwt;
        private readonly IMemoryCache memoryCache;
        private readonly IMapper mapper;

        public AccountService(UserManager<ApplicationUser> userManager, IFileHandling photoHandling,
            RoleManager<ApplicationRole> roleManager, IUnitOfWork unitOfWork,
            IOptions<Jwt> jwt, IMemoryCache _memoryCache, IMapper _mapper, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _jwt = jwt.Value;
            _fileHandling = photoHandling;
            memoryCache = _memoryCache;
            mapper = _mapper;
            _signInManager = signInManager;
        }
        //------------------------------------------------------------------------------------------------------------
        public async Task<ApplicationUser> GetUserById(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Profile)
                //.Include(u=>u.Experiences)
                .FirstOrDefaultAsync(x => x.Id == id && x.Status);
            return user;
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<List<ApplicationRole>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<List<UserRoleDTO>> GetAllUsersWithRoles()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoleDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleDTO
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList()
                });
            }

            return userRoles;
        }

        // ✅ Get users in a specific role
        public async Task<List<ApplicationUser>> GetUsersInRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return new List<ApplicationUser>();

            return (await _userManager.GetUsersInRoleAsync(roleName)).ToList();
        }

        // ✅ Assign Sales to a Manager
        public async Task<bool> AssignManagerToSalesAsync(string salesId, string managerId)
        {
            var salesUser = await _userManager.FindByIdAsync(salesId);
            var managerUser = await _userManager.FindByIdAsync(managerId);

            if (salesUser == null || managerUser == null)
                return false;

            // Assign manager's ID to a custom field (if applicable in DB)
            salesUser.ManagerId = managerId; // Assuming your user entity has `ManagerId` field
            var result = await _userManager.UpdateAsync(salesUser);

            return result.Succeeded;
        }
        //------------------------------------------------------------------------------------------------------------
        // Check if email or phone number already exists before creating or updating the user
        private async Task<bool> IsPhoneExistAsync(string Email, string userId = null)
        {
            var usersWithPhone = await _userManager.Users
            .Where(u => u.Email == Email && u.Id != userId)
            .ToListAsync();

            return (usersWithPhone.Count() == 0) ? false : true;
        }
        public async Task<bool> IsPhoneAsync(string Phone)
        {
            var usersWithPhone = await _userManager.Users
            .Where(u => u.PhoneNumber == Phone)
            .ToListAsync();

            return (usersWithPhone.Count() == 0) ? false : true;
        }

        // Helper methods for handling profile images
        private async Task SetProfileImage(ApplicationUser user, IFormFile imageProfile)
        {
            var path = await GetPathByName("ProfileImages");

            if (imageProfile != null)
            {
                user.ProfileId = await _fileHandling.UploadFile(imageProfile, path);
            }
            else
            {
                user.ProfileId = await _fileHandling.DefaultProfile(path);
            }
        }

        private async Task UpdateProfileImage(ApplicationUser user, IFormFile imageProfile)
        {
            if (imageProfile != null)
            {
                var path = await GetPathByName("ProfileImages");
                try
                {
                    user.ProfileId = await _fileHandling.UpdateFile(imageProfile, path, user.ProfileId);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to update profile image", ex);
                }
            }
        }

        public async Task<ApplicationUser> GetUserFromToken(string token)
        {
            try
            {
                var userId = ValidateJwtToken(token);
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------
        // Register methods
        public async Task<IdentityResult> RegisterSupportDeveloper(RegisterSupportDeveloper model)
        {
            var user = mapper.Map<ApplicationUser>(model);

            if (model.ImageProfile != null)
            {
                var path = await GetPathByName("ProfileImages");
                user.ProfileId = await _fileHandling.UploadFile(model.ImageProfile, path);
            }
            else
            {
                var path = await GetPathByName("ProfileImages");
                user.ProfileId = await _fileHandling.DefaultProfile(path);
            }
            user.PhoneNumberConfirmed = true;
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Support Developer");
            }
            else
            {
                // Handle potential errors by throwing an exception or logging details
                throw new InvalidOperationException("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }

        public async Task<IdentityResult> RegisterCustomer(RegisterCustomer model)
        {
            if (await IsPhoneExistAsync(model.PhoneNumber))
            {
                throw new ArgumentException("phone number already exists.");
            }

            var user = mapper.Map<ApplicationUser>(model);
            await SetProfileImage(user, model.ImageProfile);
            user.PhoneNumberConfirmed = true;

            var result = await _userManager.CreateAsync(user, "Ahmed@123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                var res = await SendOTP(user.PhoneNumber);
                if(!res)
                    throw new InvalidOperationException($"Failed to Send OTP");
            }
            else
            {
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result;
        }

        public async Task<IdentityResult> RegisterAdmin(RegisterAdmin model)
        {
            if (await IsPhoneExistAsync(model.PhoneNumber))
            {
                throw new ArgumentException("phone number already exists.");
            }

            var user = mapper.Map<ApplicationUser>(model);
            await SetProfileImage(user, model.ImageProfile);
            user.PhoneNumberConfirmed = true;
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result;
        }

        public async Task<IdentityResult> RegisterSales(RegisterSales model)
        {
            if (await IsPhoneExistAsync(model.PhoneNumber))
            {
                throw new ArgumentException("phone number already exists.");
            }

            var user = mapper.Map<ApplicationUser>(model);
            await SetProfileImage(user, model.ImageProfile);
            user.PhoneNumberConfirmed = true;
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Sales");
            }
            else
            {
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result;
        }

        public async Task<IdentityResult> Register(RegisterUser model)
        {
            if (await IsPhoneExistAsync(model.PhoneNumber))
            {
                throw new ArgumentException("Phone number already exists.");
            }

            var user = mapper.Map<ApplicationUser>(model);
            await SetProfileImage(user, model.ImageProfile);
            user.PhoneNumberConfirmed = true;

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result;
        }

        //------------------------------------------------------------------------------------------------------------
        public async Task<IdentityResult> UpdateAdmin(string adminId, RegisterAdmin model)
        {
            var user = await _userManager.FindByIdAsync(adminId);
            if (user == null)
                throw new ArgumentException("Admin not found");

            if (await IsPhoneExistAsync(model.PhoneNumber, adminId))
            {
                throw new ArgumentException("phone number already exists.");
            }

            user.PhoneNumber = model.PhoneNumber;

            await UpdateProfileImage(user, model.ImageProfile);

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateSupportDeveloper(string SupportDeveloperId, RegisterSupportDeveloper model)
        {
            var user = await _userManager.FindByIdAsync(SupportDeveloperId);
            if (user == null)
                throw new ArgumentException("Admin not found");


            if (model.ImageProfile != null)
            {
                var path = await GetPathByName("ProfileImages");
                try
                {
                    var newProfileId = await _fileHandling.UpdateFile(model.ImageProfile, path, user.ProfileId);
                    user.ProfileId = newProfileId;
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error updating file: {ex.Message}");
                    throw new InvalidOperationException("Failed to update profile image", ex);
                }
            }

            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    // Log errors
                    Console.WriteLine($"Error updating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    throw new InvalidOperationException($"Error updating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating user: {ex.Message}");
                throw new InvalidOperationException("Failed to update admin", ex);
            }
        }

        public async Task<IdentityResult> UpdateCustomer(string customerId, UpdateCustomer model)
        {
            var user = await _userManager.FindByIdAsync(customerId);
            if (user == null)
                throw new ArgumentException("Customer not found");

            if (await IsPhoneExistAsync(model.Email, customerId))
            {
                throw new ArgumentException("Email already exists.");
            }

            user.Email = model.Email;

            await UpdateProfileImage(user, model.ImageProfile);

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdatePasswordAsync(string userId, UpdatePassword updatePasswordModel)
        {
            if (updatePasswordModel == null)
                throw new ArgumentNullException(nameof(updatePasswordModel));

            // Validate user existence
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found");

            // Validate password and confirmation (already done by Data Annotations)
            if (updatePasswordModel.Password != updatePasswordModel.ConfirmPassword)
                throw new ArgumentException("Password and Confirm Password do not match");

            // Generate password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the password
            var result = await _userManager.ResetPasswordAsync(user, resetToken, updatePasswordModel.Password);

            return result;
        }

        public async Task<bool> SetDeviceTokenAsync(string userId, string deviceToken)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.DeviceToken = deviceToken;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<IdentityResult> UpdateUserDetails(UpdateUserDetails model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                throw new ArgumentException("User not found");

            // Check if email is being changed and if it already exists
            if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
            {
                if (await IsPhoneExistAsync(model.Email, model.UserId))
                {
                    throw new ArgumentException("Email already exists.");
                }
                user.Email = model.Email;
            }

            // Update full name if provided
            if (!string.IsNullOrEmpty(model.FullName))
            {
                user.FullName = model.FullName;
            }

            // Update phone number if provided
            if (!string.IsNullOrEmpty(model.PhoneNumber) && model.PhoneNumber != user.PhoneNumber)
            {
                if (await IsPhoneAsync(model.PhoneNumber))
                {
                    throw new ArgumentException("Phone number already exists.");
                }
                user.PhoneNumber = model.PhoneNumber;
                user.UserName = model.PhoneNumber; // Update username as well since it's based on phone
            }

            // Update profile image if provided
            if (model.ProfileImage != null)
            {
                await UpdateProfileImage(user, model.ProfileImage);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result;
        }

        public async Task<IdentityResult> ChangeUserPassword(ChangePasswordModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                throw new ArgumentException("User not found");

            // Verify current password
            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isCurrentPasswordValid)
                throw new ArgumentException("Current password is incorrect");

            // Change password
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to change password: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result;
        }

        public async Task<ServiceResult<string>> UpdateUserProfileImage(string userId, IFormFile profileImage)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return ServiceResult<string>.Failure("User not found");

                if (profileImage == null || profileImage.Length == 0)
                    return ServiceResult<string>.Failure("Invalid image file");

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(profileImage.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                    return ServiceResult<string>.Failure("Invalid file type. Only JPG, JPEG, PNG, and GIF files are allowed.");

                // Validate file size (5MB limit)
                if (profileImage.Length > 5 * 1024 * 1024)
                    return ServiceResult<string>.Failure("File size must be less than 5MB");

                await UpdateProfileImage(user, profileImage);
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                    return ServiceResult<string>.Failure($"Failed to update profile image: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");

                return ServiceResult<string>.Success(user.ProfileId, "Profile image updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Failure($"An error occurred: {ex.Message}");
            }
        }

        public async Task<IdentityResult> UpdateUserPhoneNumber(string userId, string phoneNumber)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            if (string.IsNullOrEmpty(phoneNumber))
                throw new ArgumentException("Phone number is required");

            // Check if phone number already exists
            if (await IsPhoneAsync(phoneNumber))
                throw new ArgumentException("Phone number already exists.");

            user.PhoneNumber = phoneNumber;
            user.UserName = phoneNumber; // Update username as well since it's based on phone
            user.PhoneNumberConfirmed = false; // Require re-verification

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update phone number: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Send OTP for verification
            await SendOTP(phoneNumber);

            return result;
        }

        public async Task<IdentityResult> UpdateUserName(string userId, string fullName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            if (string.IsNullOrEmpty(fullName))
                throw new ArgumentException("Full name is required");

            user.FullName = fullName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update name: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return result;
        }
        //------------------------------------------------------------------------------------------------------------
        public async Task<(bool IsSuccess, string Token, string ErrorMessage)> Login(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (user == null || !await _userManager.CheckPasswordAsync(user, "Ahmed@123"))
                {
                    return (false, null, "Invalid login attempt.");
                }

                // Check if user status is true and phone number is confirmed
                if (!user.Status)
                {
                    return (false, null, "Your account is deactivated. Please contact support.");
                }

                if (!user.PhoneNumberConfirmed)
                {
                    return (false, null, "Phone number not confirmed. Please verify your phone number.");
                }

                // Proceed with login
                await _signInManager.SignInAsync(user, model.RememberMe);
                var token = await GenerateJwtToken(user, true);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return (true, tokenString, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Token, string ErrorMessage)> LoginAdmin(LoginAdmin model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.PhoneNumber);
                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return (false, null, "Invalid login attempt.");
                }

                // Check if user status is true and phone number is confirmed
                if (!user.Status)
                {
                    return (false, null, "Your account is deactivated. Please contact support.");
                }

                if (!user.PhoneNumberConfirmed)
                {
                    return (false, null, "Phone number not confirmed. Please verify your phone number.");
                }

                // Proceed with login
                await _signInManager.SignInAsync(user, model.RememberMe);
                var token = await GenerateJwtToken(user, true);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return (true, tokenString, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        public async Task<bool> Logout(ApplicationUser user)
        {
            if (user == null)
                return false;

            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<bool> SendOTP(string customerPhoneNumber)
        {
            // Generate a random OTP
            var OTP = RandomOTP(6);

            // Store OTP in memory cache
            memoryCache.Set(customerPhoneNumber, OTP);

            try
            {
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }

            return false; // Return false if SMS sending fails
        }

        public async Task<bool> ValidateOTP(string customerPhoneNumber, string OTPV)
        {
            try
            {
                return true;
                // Check if the OTP exists in the memory cache
                if (memoryCache.TryGetValue(customerPhoneNumber, out string cachedOTP))
                {
                    // Compare the provided OTP with the cached OTP
                    if (cachedOTP == OTPV)
                    {
                        // OTP is valid, remove it from the cache after successful validation
                        memoryCache.Remove(customerPhoneNumber);
                        return true;
                    }
                }

                // OTP is invalid or not found
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred during OTP validation: {ex.Message}");
                return false;
            }
        }

        //------------------------------------------------------------------------------------------------------------

        public async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }
        //------------------------------------------------------------------------------------------------------------
        public async Task<IdentityResult> AssignRolesToUser(RoleUserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                throw new ArgumentException("User not found.");

            var existingRoles = await _userManager.GetRolesAsync(user);
            var validRoles = await _roleManager.Roles
                                               .Where(r => model.RoleId.Contains(r.Id))
                                               .Select(r => r.Name)
                                               .ToListAsync();

            if (!validRoles.Any())
                throw new ArgumentException("No valid roles found.");

            // Get roles to add (exclude existing ones)
            var rolesToAdd = validRoles.Except(existingRoles).ToList();

            if (rolesToAdd.Any())
            {
                var result = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!result.Succeeded)
                    throw new InvalidOperationException($"Failed to assign roles: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return IdentityResult.Success;
        }

        public Task<List<string>> GetRoles()
        {
            return _roleManager.Roles.Select(x => x.Name).ToListAsync();
        }

        public async Task<List<AuthDTO>> GetUsersWithSalesReturnRole(string UserId)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Sales");
            return mapper.Map<List<AuthDTO>>(usersInRole.Where(q => q.ManagerId == UserId));
        }
        public async Task<List<AuthDTO>> GetUsersWithSalesReturnRole()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Sales");
            return mapper.Map<List<AuthDTO>>(usersInRole);
        }

        public async Task<ApplicationUser> GetManagerWithLeastBookingsAsync()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");

            if (managers == null || !managers.Any())
                throw new InvalidOperationException("No managers found.");

            var managerBookings = new Dictionary<ApplicationUser, int>();

            foreach (var manager in managers)
            {
                var count = await _unitOfWork.BookingRepository.CountAsync(b => b.Seller.ManagerId == manager.Id);
                managerBookings[manager] = count;
            }

            return managerBookings.OrderBy(mb => mb.Value).FirstOrDefault().Key;
        }

        public async Task<bool> AssignManagerToSalesAsync(string salesId)
        {
            var salesPerson = await _unitOfWork.UserRepository.GetByIdAsync(salesId);
            if (salesPerson == null) throw new ArgumentException("Salesperson not found");

            var manager = await GetManagerWithLeastBookingsAsync();
            salesPerson.ManagerId = manager.Id;

            _unitOfWork.UserRepository.Update(salesPerson);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        //------------------------------------------------------------------------------------------------------------
        public async Task<IdentityResult> Activate(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Admin not found");

            user.Status = true;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> Suspend(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Admin not found");

            user.Status = false;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<string> GetUserProfileImage(string profileId)
        {
            if (string.IsNullOrEmpty(profileId))
            {
                return null;
            }

            var profileImage = await _fileHandling.GetFile(profileId);
            return profileImage;
        }

        //------------------------------------------------------------------------------------------------------------
        #region create and validate JWT token

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user, bool isCustomer)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim("uid", user.Id),
                new Claim(ClaimTypes.Role, isCustomer ? "Customer":"Lawyer"),
                new Claim("profileImage", await _fileHandling.GetFile(user.ProfileId)),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwt.Issuer,
                _jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddDays(Convert.ToDouble(_jwt.DurationInDays)),
                signingCredentials: creds);
            return token;
        }

        public string ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                if (token == null)
                    return null;
                if (token.StartsWith("Bearer "))
                    token = token.Replace("Bearer ", "");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.First(x => x.Type == "uid").Value;

                return accountId;
            }
            catch
            {
                return null;
            }
        }

        #endregion create and validate JWT token

        #region Random number and string

        //Generate RandomNo
        public int GenerateRandomNo()
        {
            const int min = 1000;
            const int max = 9999;
            var rdm = new Random();
            return rdm.Next(min, max);
        }

        public string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string RandomOTP(int length)
        {
            var random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion Random number and string
    }
}
