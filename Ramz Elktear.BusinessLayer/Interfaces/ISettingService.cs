using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.DTO.SettingModels;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ISettingService
    {
        Task<IEnumerable<SettingDetails>> GetAllSettingsAsync();
        Task<SettingDetails> GetSettingByIdAsync(string Id);
        Task<SettingDetails> GetSettingByTypeAsync(SettingType settingType);
        Task<SettingDetails> AddSettingAsync(AddSettingDto settingDto);
        Task<bool> UpdateSettingAsync(UpdateSettingDto settingDto);
    }
}
