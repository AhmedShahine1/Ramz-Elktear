using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Offer;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICarService _carService;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public OfferService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling, ICarService carService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
            _carService = carService;
        }

        public async Task<IEnumerable<OfferDetails>> GetAllOffersAsync()
        {
            var offers = await _unitOfWork.OfferRepository.GetAllAsync(include: q => q.Include(o => o.Car));
            var offerDTO = new List<OfferDetails>();
            foreach (var offer in offers)
            {
                offerDTO.Add(new OfferDetails()
                {
                    Id = offer.Id,
                    carDetails = await _carService.GetCarByIdAsync(offer.CarId),
                    ExpiryDate = offer.ExpiryDate,
                    CreatedAt = offer.CreatedAt,
                    NewPrice = offer.NewPrice,
                    ImageUrl = await GetOfferImage(offer.ImageId),
                });
            }
            return offerDTO;
        }

        public async Task<OfferDetails> GetOfferByIdAsync(string offerId)
        {
            var offer = await _unitOfWork.OfferRepository.FindAsync(a => a.Id == offerId, include: q => q.Include(o => o.Car));
            if (offer == null) throw new ArgumentException("Offer not found");
            var offerDTO = new OfferDetails()
            {
                Id = offer.Id,
                carDetails = await _carService.GetCarByIdAsync(offer.CarId),
                ExpiryDate = offer.ExpiryDate,
                CreatedAt = offer.CreatedAt,
                NewPrice = offer.NewPrice,
                ImageUrl =await GetOfferImage(offer.ImageId),
            };
            return offerDTO;
        }

        public async Task<OfferDetails> AddOfferAsync(AddOffer offerDto)
        {
            var offer = new Offer()
            {
                NewPrice = offerDto.NewPrice,
                CarId = offerDto.CarId,
                ExpiryDate = offerDto.ExpiryDate,
            };
            await SetOfferImage(offer,offerDto.Image);
            await _unitOfWork.OfferRepository.AddAsync(offer);
            await _unitOfWork.SaveChangesAsync();
            return await GetOfferByIdAsync(offer.Id);
        }

        public async Task<bool> DeleteOfferAsync(string offerId)
        {
            var offer = await _unitOfWork.OfferRepository.GetByIdAsync(offerId);
            if (offer == null) throw new ArgumentException("Offer not found");

            _unitOfWork.OfferRepository.Delete(offer);
            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the offer: " + ex.Message, ex);
            }
        }

        public async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }

        private async Task SetOfferImage(Offer offer, IFormFile imageProfile)
        {
            var path = await GetPathByName("ImageOffer");
            offer.ImageId = await _fileHandling.UploadFile(imageProfile, path);
        }

        private async Task UpdateOfferImage(Offer offer, IFormFile imageProfile)
        {
            var path = await GetPathByName("ImageOffer");
            offer.ImageId = await _fileHandling.UpdateFile(imageProfile, path, offer.ImageId);
        }

        public async Task<string> GetOfferImage(string offerId)
        {
            if (string.IsNullOrEmpty(offerId))
            {
                return null;
            }

            var offerImage = await _fileHandling.GetFile(offerId);
            return offerImage;
        }
    }
}
