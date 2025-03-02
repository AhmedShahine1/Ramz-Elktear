using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.PromotionModels;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.core.Entities.Promotion;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;

        public PromotionService(IUnitOfWork unitOfWork, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<PromotionDetails>> GetAllPromotionsAsync()
        {
            var promotions = await _unitOfWork.PromotionRepository.GetAllAsync();
            var promotionDTOs = new List<PromotionDetails>();

            foreach (var promotion in promotions)
            {
                promotionDTOs.Add(new PromotionDetails()
                {
                    Id = promotion.Id,
                    ImageAr = await _fileHandling.GetFile(promotion.ImageArId),
                    ImageEn = await _fileHandling.GetFile(promotion.ImageEnId),
                    redirctURL = promotion.redirctURL
                });
            }

            return promotionDTOs;
        }

        public async Task<PromotionDetails> GetPromotionByIdAsync(string promotionId)
        {
            var promotion = await _unitOfWork.PromotionRepository.GetByIdAsync(promotionId);
            if (promotion == null) throw new ArgumentException("Promotion not found");

            return new PromotionDetails
            {
                Id = promotion.Id,
                ImageAr = await _fileHandling.GetFile(promotion.ImageArId),
                ImageEn = await _fileHandling.GetFile(promotion.ImageEnId),
                redirctURL = promotion.redirctURL
            };
        }

        public async Task<PromotionDetails> AddPromotionAsync(AddPromotion promotionDto)
        {
            var promotion = new Promotion();

            promotion.ImageArId = await _fileHandling.UploadFile(promotionDto.ImageAr, await GetPathByName("Promotions"));
            promotion.ImageEnId = await _fileHandling.UploadFile(promotionDto.ImageEn, await GetPathByName("Promotions"));
            promotion.IsActive = true;
            promotion.redirctURL = promotionDto.redirctURL;
            await _unitOfWork.PromotionRepository.AddAsync(promotion);
            await _unitOfWork.SaveChangesAsync();

            return new PromotionDetails
            {
                Id = promotion.Id,
                ImageAr = await _fileHandling.GetFile(promotion.ImageArId),
                ImageEn = await _fileHandling.GetFile(promotion.ImageEnId),
                redirctURL = promotion.redirctURL
            };
        }

        public async Task<bool> UpdatePromotionAsync(string id, AddPromotion promotionDto)
        {
            var promotion = await _unitOfWork.PromotionRepository.GetByIdAsync(id);
            promotion.redirctURL = promotionDto.redirctURL;
            if (promotion == null) throw new ArgumentException("Promotion not found");

            if (promotionDto.ImageAr != null)
                promotion.ImageArId = await _fileHandling.UpdateFile(promotionDto.ImageAr,await GetPathByName("Promotions"), promotion.ImageArId);

            if (promotionDto.ImageEn != null)
                promotion.ImageEnId = await _fileHandling.UpdateFile(promotionDto.ImageEn, await GetPathByName("Promotions"), promotion.ImageEnId);

            _unitOfWork.PromotionRepository.Update(promotion);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private async Task<Paths> GetPathByName(string name)
        {
            var path = await _unitOfWork.PathsRepository.FindAsync(x => x.Name == name);
            if (path == null) throw new ArgumentException("Path not found");
            return path;
        }

    }
}
