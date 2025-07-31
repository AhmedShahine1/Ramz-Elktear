using Ramz_Elktear.core.DTO.CarOfferModels;
using Ramz_Elktear.core.DTO.OfferModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface ICarOfferService
    {
        Task<IEnumerable<OfferDTO>> GetAllOffersForCarAsync(string carId);
        Task<IEnumerable<CarOfferDTO>> GetAllCarsForOfferAsync(string offerId);
        Task<CarOfferDTO> AddCarOfferAsync(AddCarOffer carOfferDto);
        Task<bool> DeleteCarOfferAsync(string offerId, string carId);
        Task<bool> DeleteAllCarOffersAsync(string carId);
    }
}
