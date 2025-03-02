namespace Ramz_Elktear.core.DTO.CarOfferModels
{
    public class CarOfferDTO
    {
        public string OfferId { get; set; }
        public string CarId { get; set; }
        public string OfferName { get; set; }  // Name of the offer
        public string CarModel { get; set; }   // Model of the car
        public bool IsActive { get; set; }
    }
}
