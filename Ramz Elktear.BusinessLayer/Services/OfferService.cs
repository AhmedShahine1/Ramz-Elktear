using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.OfferModels;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Offer;
using Ramz_Elktear.RepositoryLayer.Interfaces;

public class OfferService : IOfferService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileHandling _fileHandling;
    private readonly ICarService _carService;

    public OfferService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling, ICarService carService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileHandling = fileHandling;
        _carService = carService;
    }

    public async Task<IEnumerable<OfferDTO>> GetAllOffersAsync()
    {
        var offers = await _unitOfWork.OffersRepository.GetAllAsync();

        var offerDTOs = offers.Select(offer =>
        {
            var offerDto = _mapper.Map<OfferDTO>(offer);
            offerDto.ImageUrl = _fileHandling.GetFile(offer.ImageId).Result;
            offerDto.CarDetails = _carService.GetCarsByOfferIdAsync(offer.Id).Result.ToList().FirstOrDefault();
            return offerDto;
        });

        return offerDTOs;
    }

    public async Task<OfferDTO> GetOfferByIdAsync(string offerId)
    {
        var offer = await _unitOfWork.OffersRepository.GetByIdAsync(offerId);
        if (offer == null) throw new ArgumentException("Offer not found");

        var offerDto = _mapper.Map<OfferDTO>(offer);
        offerDto.ImageUrl = await _fileHandling.GetFile(offer.ImageId);
        offerDto.CarDetails = _carService.GetCarsByOfferIdAsync(offerId).Result.ToList().FirstOrDefault();
        return offerDto;
    }

    public async Task<OfferDTO> AddOfferAsync(AddOffer offerDto)
    {
        var offer = _mapper.Map<Offers>(offerDto);
        if (offerDto.Image != null)
        {
            await SetOfferImage(offer, offerDto.Image);
        }

        await _unitOfWork.OffersRepository.AddAsync(offer);
        await _unitOfWork.SaveChangesAsync();

        return await GetOfferByIdAsync(offer.Id);
    }

    public async Task<bool> UpdateOfferAsync(UpdateOffer offerDto)
    {
        var offer = await _unitOfWork.OffersRepository.GetByIdAsync(offerDto.Id);
        if (offer == null) throw new ArgumentException("Offer not found");

        offer.NameAr = offerDto.NameAr;
        offer.NameEn = offerDto.NameEn;
        offer.NewPrice = offerDto.NewPrice;
        offer.StartDate = offerDto.StartDate;
        offer.EndDate = offerDto.EndDate;
        offer.delivery = offerDto.Delivery;
        if (offerDto.Image != null)
        {
            await UpdateOfferImage(offer, offerDto.Image);
        }

        _unitOfWork.OffersRepository.Update(offer);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteOfferAsync(string offerId)
    {
        var offer = await _unitOfWork.OffersRepository.GetByIdAsync(offerId);
        if (offer == null) throw new ArgumentException("Offer not found");

        _unitOfWork.OffersRepository.Delete(offer);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private async Task SetOfferImage(Offers offer, IFormFile image)
    {
        var path = await GetPathByName("OfferImages");
        offer.ImageId = await _fileHandling.UploadFile(image, path);
    }

    private async Task UpdateOfferImage(Offers offer, IFormFile newImage)
    {
        var path = await GetPathByName("OfferImages");
        offer.ImageId = await _fileHandling.UpdateFile(newImage, path, offer.ImageId);
    }

    private async Task<Paths> GetPathByName(string name)
    {
        var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
        if (path == null) throw new ArgumentException("Path not found");
        return path;
    }
}
