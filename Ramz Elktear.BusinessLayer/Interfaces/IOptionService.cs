using Ramz_Elktear.core.DTO.OptionModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IOptionService
    {
        Task<IEnumerable<OptionDTO>> GetAllOptionsAsync();
        Task<OptionDTO> GetOptionByIdAsync(string id);
        Task<OptionDTO> AddOptionAsync(CreateOptionDTO optionDto);
        Task<OptionDTO> UpdateOptionAsync(UpdateOptionDTO optionDto);
        Task<bool> DeleteOptionAsync(string id);
    }
}
