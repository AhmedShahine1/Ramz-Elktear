using Ramz_Elktear.core.Entities.Localization;

namespace Ramz_Elktear.core.DTO.LocalizationModel
{
    public class LocalizationManagementViewModel
    {
        public IEnumerable<LocalizationResource> Resources { get; set; }
        public LocalizationViewModel EditModel { get; set; }
        public LocalizationViewModel CreateModel { get; set; }
        public string SearchString { get; set; }
        public string CurrentGroup { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<string> Groups { get; set; }
    }
}
