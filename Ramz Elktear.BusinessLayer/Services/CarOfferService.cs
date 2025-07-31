using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.CarOfferModels;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.Entities.Offer;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class CarOfferService : ICarOfferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarOfferService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OfferDTO>> GetAllOffersForCarAsync(string carId)
        {
            var carOffers = await _unitOfWork.CarOfferRepository.FindAllAsync(q => q.CarId == carId, include: a => a.Include(i => i.Offer));
            return carOffers.Select(carOffer => _mapper.Map<OfferDTO>(carOffer.Offer));
        }

        public async Task<IEnumerable<CarOfferDTO>> GetAllCarsForOfferAsync(string offerId)
        {
            var carOffers = await _unitOfWork.CarOfferRepository.FindAllAsync(q => q.OfferId == offerId);
            if (carOffers == null || !carOffers.Any()) throw new ArgumentException("No cars found for the given offerId");

            return carOffers.Select(carOffer => _mapper.Map<CarOfferDTO>(carOffer));
        }

        public async Task<CarOfferDTO> AddCarOfferAsync(AddCarOffer carOfferDto)
        {
            var carOffer = new CarOffer
            {
                OfferId = carOfferDto.OfferId,
                CarId = carOfferDto.CarId,
                SellingPrice = carOfferDto.SellingPrice,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            await _unitOfWork.CarOfferRepository.AddAsync(carOffer);
            await _unitOfWork.SaveChangesAsync();

            // Returning the added CarOffer
            return _mapper.Map<CarOfferDTO>(carOffer);
        }

        public async Task<bool> DeleteCarOfferAsync(string offerId, string carId)
        {
            var carOffer = await _unitOfWork.CarOfferRepository.FindAllAsync(q => q.OfferId == offerId && q.CarId == carId);
            if (carOffer == null) throw new ArgumentException("CarOffer not found for the given offerId and carId");

            _unitOfWork.CarOfferRepository.DeleteRange(carOffer);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAllCarOffersAsync(string carId)
        {
            var carOffers = await _unitOfWork.CarOfferRepository.FindAllAsync(q => q.CarId == carId);
            if (carOffers != null && carOffers.Any())
            {
                _unitOfWork.CarOfferRepository.DeleteRange(carOffers);
                await _unitOfWork.SaveChangesAsync();
            }
            return true;
        }
    }
}
