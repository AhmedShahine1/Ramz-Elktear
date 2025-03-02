using AutoMapper;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.ColorModels;
using Ramz_Elktear.core.Entities.Color;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IMapper _mapper;

        public ColorService(IUnitOfWork unitOfWork, IMapper mapper, IFileHandling fileHandling)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileHandling = fileHandling;
        }

        public async Task<IEnumerable<ColorDTO>> GetAllColorsAsync()
        {
            var colors = await _unitOfWork.ColorsRepository.GetAllAsync();
            return colors.Select(c => _mapper.Map<ColorDTO>(c));
        }

        public async Task<ColorDTO> GetColorByIdAsync(string id)
        {
            var color = await _unitOfWork.ColorsRepository.GetByIdAsync(id);
            if (color == null) throw new ArgumentException("Color not found");

            return _mapper.Map<ColorDTO>(color);
        }

        public async Task<ColorDTO> AddColorAsync(AddColor colorDto)
        {
            var color = new Colors
            {
                Name = colorDto.Name,
                Value = colorDto.Value,
                CreatedDate = DateTime.UtcNow,
                IsActive = colorDto.IsActive
            };

            await _unitOfWork.ColorsRepository.AddAsync(color);
            await _unitOfWork.SaveChangesAsync();

            return await GetColorByIdAsync(color.Id);
        }

        public async Task<ColorDTO> UpdateColorAsync(UpdateColor colorDto)
        {
            var color = await _unitOfWork.ColorsRepository.GetByIdAsync(colorDto.Id);
            if (color == null) throw new ArgumentException("Color not found");

            color.Name = colorDto.Name;
            color.Value = colorDto.Value;
            color.LastModifiedDate = DateTime.UtcNow;
            color.IsActive = colorDto.IsActive;

            _unitOfWork.ColorsRepository.Update(color);
            await _unitOfWork.SaveChangesAsync();

            return await GetColorByIdAsync(color.Id);
        }

        public async Task<bool> DeleteColorAsync(string id)
        {
            var color = await _unitOfWork.ColorsRepository.GetByIdAsync(id);
            if (color == null) throw new ArgumentException("Color not found");

            _unitOfWork.ColorsRepository.Delete(color);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
