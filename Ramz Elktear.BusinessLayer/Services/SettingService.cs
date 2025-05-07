using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.SettingModels;
using Ramz_Elktear.core.Entities;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Helper;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class SettingService : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileHandling _fileHandling;

        public SettingService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        // ✅ Get All Settings
        public async Task<IEnumerable<SettingDetails>> GetAllSettingsAsync()
        {
            var settings = await _unitOfWork.SettingsRepository.GetAllAsync();
            var settingDTOs = new List<SettingDetails>();

            foreach (var setting in settings)
            {
                settingDTOs.Add(new SettingDetails()
                {
                    Id = setting.Id,
                    ImageType = setting.ImageType,
                    ImageUrl = await GetSettingImage(setting.ImageUrlId),
                });
            }

            return settingDTOs;
        }
        public async Task<SettingDetails> GetSettingByIdAsync(string Id)
        {
            var settings = await _unitOfWork.SettingsRepository.GetByIdAsync(Id);
            var settingDTOs = new SettingDetails{
                    Id = settings.Id,
                    ImageType = settings.ImageType,
                    ImageUrl = await GetSettingImage(settings.ImageUrlId),
                };

            return settingDTOs;
        }

        // ✅ Get Setting By Type (Login, Register, Logo)
        public async Task<SettingDetails> GetSettingByTypeAsync(SettingType settingType)
        {
            var setting = await _unitOfWork.SettingsRepository.FindAsync(x => x.ImageType == settingType.ToString());
            if (setting == null) throw new ArgumentException("Setting not found");

            return new SettingDetails()
            {
                Id = setting.Id,
                ImageType = setting.ImageType,
                ImageUrl = await GetSettingImage(setting.ImageUrlId),
            };
        }

        // ✅ Add a New Setting
        public async Task<SettingDetails> AddSettingAsync(AddSettingDto settingDto)
        {
            var setting = new Setting
            {
                ImageType = settingDto.ImageType.ToString(),
            };

            if (settingDto.Image != null)
            {
                await SetSettingImage(setting, settingDto.Image);
            }

            await _unitOfWork.SettingsRepository.AddAsync(setting);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SettingDetails>(setting);
        }

        // ✅ Update Setting (Change Image)
        public async Task<bool> UpdateSettingAsync(UpdateSettingDto settingDto)
        {
            var setting = await _unitOfWork.SettingsRepository.GetByIdAsync(settingDto.Id);
            if (setting == null) throw new ArgumentException("Setting not found");

            _mapper.Map(settingDto, setting);

            if (settingDto.Image != null)
            {
                await UpdateSettingImage(setting, settingDto.Image);
            }

            _unitOfWork.SettingsRepository.Update(setting);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the setting: " + ex.Message, ex);
            }
        }


        private async Task SetSettingImage(Setting branch, IFormFile image)
        {
            var path = await GetPathByName("ImageSetting");
            branch.ImageUrlId = await _fileHandling.UploadFile(image, path);
            branch.ImageUrl = await _unitOfWork.ImagesRepository.GetByIdAsync(branch.ImageUrlId);
        }

        private async Task UpdateSettingImage(Setting branch, IFormFile newImage)
        {
            var path = await GetPathByName("ImageSetting");
            branch.ImageUrlId = await _fileHandling.UpdateFile(newImage, path, branch.ImageUrlId);
        }

        private async Task<string> GetSettingImage(string imageId)
        {
            if (string.IsNullOrEmpty(imageId))
            {
                return null;
            }

            return await _fileHandling.GetFile(imageId);
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }

    }
}
