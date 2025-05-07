using AutoMapper;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.core.Helper;
using Ramz_Elktear.RepositoryLayer.Interfaces;
using System;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<JobDTO>> GetAllAsync()
        {
            var jobs = await _unitOfWork.JobRepository.GetAllAsync();
            return jobs.Select(j => new JobDTO
            {
                Id = j.Id,
                Name = j.Name,
                Description = j.Description,
                IsConvertable = j.IsConvertable,
                Percentage = j.Percentage
            });
        }

        public async Task<JobDTO> GetByIdAsync(string id)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(id);
            if (job == null) return null;

            return new JobDTO
            {
                Id = job.Id,
                Name = job.Name,
                Description = job.Description,
                IsConvertable = job.IsConvertable,
                Percentage = job.Percentage

            };
        }

        public async Task<bool> AddAsync(AddJobDTO dto)
        {
            var job = new Job
            {
                Name = dto.Name,
                Description = dto.Description,
                IsConvertable = dto.IsConvertable,
                Percentage = dto.Percentage
            };

            await _unitOfWork.JobRepository.AddAsync(job);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(JobDTO dto)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(dto.Id);
            if (job == null) return false;

            job.Name = dto.Name;
            job.Description = dto.Description;
            job.IsConvertable = dto.IsConvertable;
            job.Percentage = dto.Percentage;

            _unitOfWork.JobRepository.Update(job);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(id);
            if (job == null) return false;

            _unitOfWork.JobRepository.Delete(job);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
