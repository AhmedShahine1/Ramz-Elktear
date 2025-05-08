using Ramz_Elktear.core.DTO.LocalizationModel;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ILocalizationService
    {
        Task<LocalizationManagementViewModel> GetLocalizationResourcesAsync(string searchString, string group, int page = 1, int pageSize = 20);
        Task<LocalizationViewModel> GetLocalizationResourceByIdAsync(int id);
        Task<bool> CreateLocalizationResourceAsync(LocalizationViewModel model, string userId);
        Task<bool> UpdateLocalizationResourceAsync(LocalizationViewModel model, string userId);
        Task<bool> DeleteLocalizationResourceAsync(int id);
        void RefreshLocalizationCache();
        Task<bool> ImportJsonDataAsync(string jsonContent, string userId);
        Task<bool> ImportCsvDataAsync(string csvContent, string userId);
        Task<(byte[] FileContents, string ContentType, string FileName)> ExportResourcesAsync(string format);
        Task<int> ScanMissingKeysAsync(string userId);
    }
}
