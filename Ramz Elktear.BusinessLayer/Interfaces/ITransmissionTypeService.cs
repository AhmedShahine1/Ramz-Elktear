using Ramz_Elktear.core.DTO.TransmissionTypeModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ITransmissionTypeService
    {
        Task<IEnumerable<TransmissionTypeDTO>> GetAllTransmissionTypesAsync();
        Task<TransmissionTypeDTO> GetTransmissionTypeByIdAsync(string id);
        Task<TransmissionTypeDTO> AddTransmissionTypeAsync(AddTransmissionType transmissionTypeDto);
        Task<TransmissionTypeDTO> UpdateTransmissionTypeAsync(UpdateTransmissionType transmissionTypeDto);
        Task<bool> DeleteTransmissionTypeAsync(string id);
    }
}
