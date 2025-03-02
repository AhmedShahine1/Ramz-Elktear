using Ramz_Elktear.core.DTO.OfferModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IOfferService
    {
        Task<IEnumerable<OfferDetails>> GetAllOffersAsync();
        Task<OfferDetails> GetOfferByIdAsync(string offerId);
        Task<OfferDetails> AddOfferAsync(AddOffer offerDto);
        Task<bool> DeleteOfferAsync(string offerId);
    }
}
