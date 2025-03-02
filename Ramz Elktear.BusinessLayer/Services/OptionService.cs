using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.OptionModels;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.core.Entities.Files;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class OptionService : IOptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OptionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OptionDTO>> GetAllOptionsAsync()
        {
            var options = await _unitOfWork.OptionRepository.GetAllAsync();
            return options.Select(o => _mapper.Map<OptionDTO>(o));
        }

        public async Task<OptionDTO> GetOptionByIdAsync(string id)
        {
            var option = await _unitOfWork.OptionRepository.GetByIdAsync(id);
            if (option == null) throw new ArgumentException("Option not found");
            return _mapper.Map<OptionDTO>(option);
        }

        public async Task<OptionDTO> AddOptionAsync(CreateOptionDTO optionDto)
        {
            var option = new Option
            {
                NameAr = optionDto.NameAr,
                NameEn = optionDto.NameEn,
                DescriptionAr = optionDto.DescriptionAr,
                DescriptionEn = optionDto.DescriptionEn,
                CreatedDate = DateTime.UtcNow,
                IsActive = optionDto.IsActive
            };

            await _unitOfWork.OptionRepository.AddAsync(option);
            await _unitOfWork.SaveChangesAsync();

            return await GetOptionByIdAsync(option.Id);
        }

        public async Task<OptionDTO> UpdateOptionAsync(UpdateOptionDTO optionDto)
        {
            var option = await _unitOfWork.OptionRepository.GetByIdAsync(optionDto.Id);
            if (option == null) throw new ArgumentException("Option not found");

            option.NameAr = optionDto.NameAr;
            option.NameEn = optionDto.NameEn;
            option.DescriptionAr = optionDto.DescriptionAr;
            option.DescriptionEn = optionDto.DescriptionEn;
            option.LastModifiedDate = DateTime.UtcNow;
            option.IsActive = optionDto.IsActive;

            _unitOfWork.OptionRepository.Update(option);
            await _unitOfWork.SaveChangesAsync();

            return await GetOptionByIdAsync(option.Id);
        }

        public async Task<bool> DeleteOptionAsync(string id)
        {
            var option = await _unitOfWork.OptionRepository.GetByIdAsync(id);
            if (option == null) throw new ArgumentException("Option not found");

            _unitOfWork.OptionRepository.Delete(option);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
