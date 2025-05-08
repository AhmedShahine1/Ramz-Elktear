using Ramz_Elktear.core.Entities.Localization;

namespace Ramz_Elktear.core.DTO.LocalizationModel
{
    public class LocalizationViewModel
    {
        public LocalizationResourceDto Resource { get; set; }
        public List<LocalizationValueViewModel> Values { get; set; }
    }
}
