using AutoMapper;
using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.ModelYearModels;
using Ramz_Elktear.core.Entities.Cars;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class ModelYearService : IModelYearService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModelYearService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModelYearDTO>> GetAllModelYearsAsync()
        {
            var modelYears = await _unitOfWork.ModelYearRepository.GetAllAsync();
            return modelYears.Select(my => _mapper.Map<ModelYearDTO>(my));
        }

        public async Task<ModelYearDTO> GetModelYearByIdAsync(string id)
        {
            var modelYear = await _unitOfWork.ModelYearRepository.GetByIdAsync(id);
            if (modelYear == null) throw new ArgumentException("ModelYear not found");

            return _mapper.Map<ModelYearDTO>(modelYear);
        }

        public async Task<ModelYearDTO> AddModelYearAsync(AddModelYear modelYearDto)
        {
            var modelYear = new ModelYear
            {
                Name = modelYearDto.Name,
                CreatedDate = DateTime.UtcNow,
                IsActive = modelYearDto.IsActive
            };

            await _unitOfWork.ModelYearRepository.AddAsync(modelYear);
            await _unitOfWork.SaveChangesAsync();

            return await GetModelYearByIdAsync(modelYear.Id);
        }

        public async Task<ModelYearDTO> UpdateModelYearAsync(UpdateModelYear modelYearDto)
        {
            var modelYear = await _unitOfWork.ModelYearRepository.GetByIdAsync(modelYearDto.Id);
            if (modelYear == null) throw new ArgumentException("ModelYear not found");

            modelYear.Name = modelYearDto.Name;
            modelYear.LastModifiedDate = DateTime.UtcNow;
            modelYear.IsActive = modelYearDto.IsActive;

            _unitOfWork.ModelYearRepository.Update(modelYear);
            await _unitOfWork.SaveChangesAsync();

            return await GetModelYearByIdAsync(modelYear.Id);
        }

        public async Task<bool> DeleteModelYearAsync(string id)
        {
            var modelYear = await _unitOfWork.ModelYearRepository.GetByIdAsync(id);
            if (modelYear == null) throw new ArgumentException("ModelYear not found");

            _unitOfWork.ModelYearRepository.Delete(modelYear);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
