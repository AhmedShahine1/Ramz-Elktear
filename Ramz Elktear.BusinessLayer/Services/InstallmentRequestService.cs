using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.AuthModels;
using Ramz_Elktear.core.DTO.CarModels;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.DTO;
using Ramz_Elktear.core.DTO.InstallmentRequestModels;
using Ramz_Elktear.core.DTO.RegisterModels;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using Ramz_Elktear.core.Helper;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.core.Entities.ApplicationData;
using Ramz_Elktear.core.DTO.ModelYearModels;
using Ramz_Elktear.core.DTO.BookingModels;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class InstallmentRequestService : IInstallmentRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountService _accountService;
        private readonly ICarService _carService;

        public InstallmentRequestService(IUnitOfWork unitOfWork, IAccountService accountService, ICarService carService)
        {
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _carService = carService;
        }

        /// <summary>
        /// Filters installment requests based on various parameters.
        /// </summary>
        public async Task<IEnumerable<DetailsInstallmentRequest>> FilterInstallmentRequests(InstallmentRequestFilter filter)
        {
            var query = _unitOfWork.InstallmentRequestsRepository.FindByQuery(x =>
                (string.IsNullOrEmpty(filter.Id) || x.Id == filter.Id) &&
                (string.IsNullOrEmpty(filter.UserId) || x.UserId == filter.UserId) &&
                (string.IsNullOrEmpty(filter.CarId) || x.CarId == filter.CarId) &&
                (string.IsNullOrEmpty(filter.BankId) || x.BankId == filter.BankId) &&
                (string.IsNullOrEmpty(filter.JobId) || x.JobId == filter.JobId) &&
                (!filter.Gender.HasValue || x.gender == filter.Gender.ToString()) &&
                (!filter.Status.HasValue || x.Status == filter.Status.ToString()),
                include: q => q
                .Include(a => a.User)
                .Include(a => a.Bank)
                .Include(a => a.Job)
                );
            var requests = query.ToList();

            return requests.Select(x => new DetailsInstallmentRequest
            {
                Id = x.Id,
                User = new AuthDTO { Id = x.User.Id, FullName = x.User.FullName, PhoneNumber = x.User.PhoneNumber },
                Bank = new BankDetails { Id = x.Bank.Id, Name = x.Bank.Name },
                Job = new JobDTO { Id = x.Job.Id, Name = x.Job.Name },
                Car = _carService.GetCarByIdAsync(x.CarId).Result, // ⚠️ Avoid using .Result in async calls
                MonthlyIncome = x.MonthlyIncome,
                MonthlyObligations = x.MonthlyObligations,
                Status = Enum.TryParse<InstallmentStatus>(x.Status, out var statusEnum) ? statusEnum : InstallmentStatus.Pending,
                Gender = Enum.TryParse<Gender>(x.gender, out var genderEnum) ? genderEnum : Gender.Male,
                CreatedAt = x.CreatedAt,
                InstallmentMonths = x.InstallmentMonths,
                InstallmentPrice = x.InstallmentPrice
            }).ToList();
         }


        /// <summary>
        /// Adds an installment request, ensuring the user exists (or registers them if they don't).
        /// </summary>
        /// 
        public async Task<DetailsInstallmentRequest> AddInstallmentRequestWithUserCheck(AddInstallmentRequest model)
        {
            try
            {
                // 🔹 Step 1: Check if user exists by phone number
                var existingUser = await _accountService.IsPhoneAsync(model.PhoneNumber);
                string userId;

                if (existingUser)
                {
                    var loginModel = new LoginModel { PhoneNumber = model.PhoneNumber };
                    var loginResult = await _accountService.Login(loginModel);
                    if (!loginResult.IsSuccess)
                    {
                        throw new Exception("Login failed after registration");
                    }
                    var user = await _accountService.GetUserFromToken(loginResult.Token);
                    if (user == null)
                        throw new Exception("User not found.");

                    userId = user.Id;
                }
                else
                {
                    // 🔹 Step 2: Register new user
                    var registerModel = new RegisterCustomer
                    {
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber
                    };

                    var registerResult = await _accountService.RegisterCustomer(registerModel);

                    if (!registerResult.Succeeded)
                        throw new Exception("User registration failed: " + string.Join(", ", registerResult.Errors.Select(e => e.Description)));

                    var loginModel = new LoginModel { PhoneNumber = model.PhoneNumber };
                    var loginResult = await _accountService.Login(loginModel);
                    if (!loginResult.IsSuccess)
                    {
                        throw new Exception("Login failed after registration");
                    }
                    var user = await _accountService.GetUserFromToken(loginResult.Token);
                    if (user == null)
                        throw new Exception("Failed to retrieve newly registered user.");

                    userId = user.Id;
                }

                // 🔹 Step 3: Fetch or Create Insurance Percentage
                var age = int.Parse(model.Age);
                var insurance = await _unitOfWork.InsurancePercentageRepository.FindAsync(
                    i => i.MinAge <= age && i.MaxAge >= age && i.Gender == model.Gender.ToString()
                );

                if (insurance == null)
                {
                    insurance = await _unitOfWork.InsurancePercentageRepository.FindAsync(
                        i => i.Gender == model.Gender.ToString(),
                        orderBy: q => q.OrderByDescending(i => i.MaxAge)
                    );
                }

                // 🔹 Step 5: Create Installment Request
                var installmentRequest = new InstallmentRequest
                {
                    UserId = userId,
                    CarId = model.CarId,
                    Car = _unitOfWork.CarRepository.GetById(model.CarId),
                    BankId = model.BankId,
                    Bank = _unitOfWork.BankRepository.GetById(model.BankId),
                    JobId = model.JobId,
                    Job = _unitOfWork.JobRepository.GetById(model.JobId),
                    MonthlyIncome = model.MonthlyIncome,
                    MonthlyObligations = model.MonthlyObligations,
                    InstallmentMonths = model.InstallmentMonths,
                    InsurancePercentageId = insurance.Id,
                    InsurancePercentage = insurance,
                    CreatedAt = DateTime.UtcNow
                };

                installmentRequest.CalculateInstallmentPrice();

                // 🔹 Step 6: Save to Database
                await _unitOfWork.InstallmentRequestsRepository.AddAsync(installmentRequest);
                await _unitOfWork.SaveChangesAsync();

                // 🔹 Step 7: Fetch Related Data for Response
                var userEntity = await _accountService.GetUserById(userId);
                var car = await _carService.GetCarByIdAsync(installmentRequest.CarId);
                var bank = await _unitOfWork.BankRepository.GetByIdAsync(installmentRequest.BankId);
                var job = await _unitOfWork.JobRepository.GetByIdAsync(installmentRequest.JobId);

                return new DetailsInstallmentRequest
                {
                    Id = installmentRequest.Id,
                    User = new AuthDTO
                    {
                        Id = userEntity.Id,
                        FullName = userEntity.FullName,
                        PhoneNumber = userEntity.PhoneNumber
                    },
                    Bank = new BankDetails { Id = bank.Id, Name = bank.Name },
                    Job = new JobDTO { Id = job.Id, Name = job.Name },
                    Car = car,
                    MonthlyIncome = installmentRequest.MonthlyIncome,
                    MonthlyObligations = installmentRequest.MonthlyObligations,
                    Status = Enum.Parse<InstallmentStatus>(installmentRequest.Status),
                    Gender = Enum.Parse<Gender>(installmentRequest.gender),
                    CreatedAt = installmentRequest.CreatedAt,
                    InstallmentMonths = installmentRequest.InstallmentMonths,
                    InstallmentPrice = installmentRequest.InstallmentPrice
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add installment request: " + ex.Message);
            }
        }

        public async Task<DetailsInstallmentRequest> CheckInstallmentWithoutRequest(AddInstallmentRequest model)
        {
            try
            {
                // 🔹 Step 1: التحقق مما إذا كان المستخدم موجودًا
                var existingUser = await _accountService.IsPhoneAsync(model.PhoneNumber);
                string userId = null;
                ApplicationUser user = null;

                if (existingUser)
                {
                    var loginModel = new LoginModel { PhoneNumber = model.PhoneNumber };
                    var loginResult = await _accountService.Login(loginModel);
                    if (!loginResult.IsSuccess)
                    {
                        throw new Exception("Login failed.");
                    }
                    user = await _accountService.GetUserFromToken(loginResult.Token);
                    if (user == null)
                        throw new Exception("User not found.");

                    userId = user.Id;
                }
                else
                {
                    // 🔹 User does not exist, register them
                    var registerModel = new RegisterCustomer
                    {
                        FullName = model.FullName,
                        PhoneNumber = model.PhoneNumber
                    };
                    var registerResult = await _accountService.RegisterCustomer(registerModel);

                    if (!registerResult.Succeeded)
                    {
                        throw new Exception("User registration failed.");
                    }

                    // 🔹 Login newly registered user
                    var loginModel = new LoginModel { PhoneNumber = model.PhoneNumber };
                    var loginResult = await _accountService.Login(loginModel);

                    if (!loginResult.IsSuccess)
                    {
                        throw new Exception("Login failed after registration");
                    }
                    user = await _accountService.GetUserFromToken(loginResult.Token);
                }
                userId = user.Id;

                // 🔹 Step 2: الحصول على بيانات السيارة، البنك، والوظيفة
                var car = await _carService.GetCarByIdAsync(model.CarId);
                var bank = await _unitOfWork.BankRepository.GetByIdAsync(model.BankId);
                var job = await _unitOfWork.JobRepository.GetByIdAsync(model.JobId);

                if (car == null || bank == null || job == null)
                {
                    throw new Exception("Invalid car, bank, or job selection.");
                }

                // 🔹 Step 3: جلب نسبة التأمين بناءً على العمر والجنس
                var age = int.Parse(model.Age);
                var insurance = await _unitOfWork.InsurancePercentageRepository.FindAsync(
                    i => i.MinAge <= age && i.MaxAge >= age && i.Gender == model.Gender.ToString()
                );

                if (insurance == null)
                {
                    insurance = await _unitOfWork.InsurancePercentageRepository.FindAsync(
                        i => i.Gender == model.Gender.ToString(),
                        orderBy: q => q.OrderByDescending(i => i.MaxAge)
                    );
                }

                // 🔹 Step 4: إنشاء كائن مؤقت لحساب القسط
                var installmentRequest = new InstallmentRequest
                {
                    UserId = userId,
                    CarId = model.CarId,
                    Car = _unitOfWork.CarRepository.GetById(model.CarId),
                    BankId = model.BankId,
                    Bank = _unitOfWork.BankRepository.GetById(model.BankId),
                    JobId = model.JobId,
                    Job = _unitOfWork.JobRepository.GetById(model.JobId),
                    MonthlyIncome = model.MonthlyIncome,
                    MonthlyObligations = model.MonthlyObligations,
                    InstallmentMonths = model.InstallmentMonths,
                    InsurancePercentageId = insurance.Id,
                    InsurancePercentage = insurance,
                    CreatedAt = DateTime.UtcNow
                };

                // 🔹 Step 5: حساب القسط مع تعيين الحالة "Checked"
                installmentRequest.CalculateInstallmentPrice();
                installmentRequest.Status = InstallmentStatus.Checked.ToString();
                await _unitOfWork.InstallmentRequestsRepository.AddAsync(installmentRequest);
                await _unitOfWork.SaveChangesAsync();

                // 🔹 Step 6: إرجاع النتيجة دون حفظها في قاعدة البيانات
                return new DetailsInstallmentRequest
                {
                    Id = installmentRequest.Id,
                    User = user != null ? new AuthDTO
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        PhoneNumber = user.PhoneNumber
                    } : null,
                    Bank = new BankDetails { Id = bank.Id, Name = bank.Name },
                    Job = new JobDTO { Id = job.Id, Name = job.Name },
                    Car = car,
                    MonthlyIncome = installmentRequest.MonthlyIncome,
                    MonthlyObligations = installmentRequest.MonthlyObligations,
                    Status = Enum.Parse<InstallmentStatus>(installmentRequest.Status),
                    Gender = Enum.Parse<Gender>(installmentRequest.gender),
                    CreatedAt = installmentRequest.CreatedAt,
                    InstallmentMonths = installmentRequest.InstallmentMonths,
                    InstallmentPrice = installmentRequest.InstallmentPrice
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to check installment: " + ex.Message);
            }
        }

        public async Task<DetailsInstallmentRequest> UpdateInstallmentRequest(UpdateInstallmentRequest model)
        {
            var installmentRequest = await _unitOfWork.InstallmentRequestsRepository.GetByIdAsync(model.Id);
            if (installmentRequest == null)
                throw new Exception("Installment request not found.");

            installmentRequest.CarId = model.CarId;
            installmentRequest.BankId = model.BankId;
            installmentRequest.JobId = model.JobId;
            installmentRequest.MonthlyIncome = model.MonthlyIncome;
            installmentRequest.MonthlyObligations = model.MonthlyObligations;
            installmentRequest.InstallmentMonths = model.InstallmentMonths;

            installmentRequest.CalculateInstallmentPrice(); // Recalculate installment price if needed

            _unitOfWork.InstallmentRequestsRepository.Update(installmentRequest);
            await _unitOfWork.SaveChangesAsync();

            // Retrieve related data for the response
            var userEntity = await _accountService.GetUserById(installmentRequest.UserId);
            var car = await _carService.GetCarByIdAsync(installmentRequest.CarId);
            var bank = await _unitOfWork.BankRepository.GetByIdAsync(installmentRequest.BankId);
            var job = await _unitOfWork.JobRepository.GetByIdAsync(installmentRequest.JobId);

            return new DetailsInstallmentRequest
            {
                User = new AuthDTO
                {
                    Id = userEntity.Id,
                    FullName = userEntity.FullName,
                    PhoneNumber = userEntity.PhoneNumber
                },
                Bank = new BankDetails { Id = bank.Id, Name = bank.Name },
                Job = new JobDTO { Id = job.Id, Name = job.Name },
                Car = car,
                MonthlyIncome = installmentRequest.MonthlyIncome,
                MonthlyObligations = installmentRequest.MonthlyObligations,
                Status = Enum.Parse<InstallmentStatus>(installmentRequest.Status),
                Gender = Enum.Parse<Gender>(installmentRequest.gender),
                CreatedAt = installmentRequest.CreatedAt,
                InstallmentMonths = installmentRequest.InstallmentMonths
            };
        }

        public async Task<bool> DeleteInstallmentRequest(string id)
        {
            var installmentRequest = await _unitOfWork.InstallmentRequestsRepository.GetByIdAsync(id);
            if (installmentRequest == null)
                return false;

            _unitOfWork.InstallmentRequestsRepository.Delete(installmentRequest);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateInstallmentRequestStatus(string id, InstallmentStatus newStatus)
        {
            var installmentRequest = await _unitOfWork.InstallmentRequestsRepository.GetByIdAsync(id);
            if (installmentRequest == null)
                throw new Exception("Installment request not found.");

            installmentRequest.Status = newStatus.ToString();
            _unitOfWork.InstallmentRequestsRepository.Update(installmentRequest);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<DataInstallmentRequest> DataInstallmentRequest()
        {
            // Fetch and transform Car data with its ModelYear details
            var car = await _unitOfWork.CarRepository.FindAllAsync(
                q => q.IsActive && !q.IsDeleted,
                include: a => a.Include(w => w.ModelYear), // Including ModelYear entity
                orderBy: null // Optional: if you want any specific ordering
            );

            var carDto = car.Select(c => new CarDTO
            {
                Id = c.Id,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                ModelYear = c.ModelYear != null ? new ModelYearDTO
                {
                    Id = c.ModelYear.Id,
                    Name = c.ModelYear.Name,
                    IsActive = c.ModelYear.IsActive
                } : null // Ensure that ModelYear is not null before mapping to DTO
            }).ToList();

            // Fetch and transform Bank data into DTOs
            var bank = await _unitOfWork.BankRepository.GetAllAsync();
            var bankDto = bank.Select(b => new BankDetails
            {
                Id = b.Id,
                Name = b.Name
            }).ToList();

            // Fetch and transform Job data into DTOs
            var job = await _unitOfWork.JobRepository.GetAllAsync();
            var jobDto = job.Select(j => new JobDTO
            {
                Id = j.Id,
                Name = j.Name,
                IsConvertable = j.IsConvertable,
            }).ToList();

            // Return the transformed data in DataInstallmentRequest
            return new DataInstallmentRequest
            {
                Car = carDto,
                Bank = bankDto,
                Job = jobDto
            };
        }

    }
}
