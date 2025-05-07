using Ramz_Elktear.BusinessLayer.Interfaces;
using Ramz_Elktear.core.DTO.InsurancePercentageModels;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.core.Helper;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class InsurancePercentageService : IInsurancePercentageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InsurancePercentageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InsurancePercentageDTO>> GetAllAsync()
        {
            var entities = await _unitOfWork.InsurancePercentageRepository.GetAllAsync();
            return entities.Select(e => new InsurancePercentageDTO
            {
                Id = e.Id,
                MinAge = e.MinAge,
                MaxAge = e.MaxAge,
                Gender = Enum.TryParse<Gender>(e.Gender, out var gender) ? gender : Gender.Male, // Default to Male
                Percentage = e.Percentage
            });
        }

        public async Task<InsurancePercentageDTO> GetByIdAsync(string id)
        {
            var entity = await _unitOfWork.InsurancePercentageRepository.GetByIdAsync(id);
            if (entity == null) return null;

            return new InsurancePercentageDTO
            {
                Id = entity.Id,
                MinAge = entity.MinAge,
                MaxAge = entity.MaxAge,
                Gender = Enum.TryParse<Gender>(entity.Gender, out var gender) ? gender : Gender.Male, // Default to Male
                Percentage = entity.Percentage
            };
        }

        public async Task<bool> AddAsync(AddInsurancePercentageDTO dto)
        {
            var entity = new InsurancePercentage
            {
                MinAge = dto.MinAge,
                MaxAge = dto.MaxAge,
                Gender = dto.Gender.ToString(),
                Percentage = dto.Percentage
            };

            await _unitOfWork.InsurancePercentageRepository.AddAsync(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(InsurancePercentageDTO dto)
        {
            var entity = await _unitOfWork.InsurancePercentageRepository.GetByIdAsync(dto.Id);
            if (entity == null) return false;

            entity.MinAge = dto.MinAge;
            entity.MaxAge = dto.MaxAge;
            entity.Gender = dto.Gender.ToString();
            entity.Percentage = dto.Percentage;

            _unitOfWork.InsurancePercentageRepository.Update(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _unitOfWork.InsurancePercentageRepository.GetByIdAsync(id);
            if (entity == null) return false;

            _unitOfWork.InsurancePercentageRepository.Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}