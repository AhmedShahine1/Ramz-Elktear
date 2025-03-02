using Ramz_Elktear.core.DTO.OfferModels;
using Microsoft.AspNetCore.Http;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IOfferService
    {
        Task<IEnumerable<OfferDTO>> GetAllOffersAsync();
        Task<OfferDTO> GetOfferByIdAsync(string offerId);
        Task<OfferDTO> AddOfferAsync(AddOffer offerDto);
        Task<bool> UpdateOfferAsync(UpdateOffer offerDto);
        Task<bool> DeleteOfferAsync(string offerId);
    }
}
