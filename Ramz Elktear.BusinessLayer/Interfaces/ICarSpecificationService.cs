using Ramz_Elktear.core.DTO.CarSpecificationModels;
using Ramz_Elktear.core.DTO.SpecificationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ICarSpecificationService
    {
        Task<IEnumerable<SpecificationDTO>> GetSpecificationsByCarIdAsync(string carId);
        Task<IEnumerable<CarSpecificationDTO>> GetCarsBySpecificationIdAsync(string specificationId);
        Task<CarSpecificationDTO> AddCarSpecificationAsync(AddCarSpecification carSpecificationDto);
        Task<bool> DeleteCarSpecificationAsync(string carId, string specificationId);
    }
}
