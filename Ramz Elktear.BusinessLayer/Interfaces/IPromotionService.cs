using Ramz_Elktear.core.DTO.PromotionModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IPromotionService
    {
        Task<bool> UpdatePromotionAsync(string id, AddPromotion promotionDto);
        Task<PromotionDetails> AddPromotionAsync(AddPromotion promotionDto);
        Task<PromotionDetails> GetPromotionByIdAsync(string promotionId);
        Task<IEnumerable<PromotionDetails>> GetAllPromotionsAsync();
    }
}
