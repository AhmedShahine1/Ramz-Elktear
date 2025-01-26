using AutoMapper;
using Ramz_Elktear.core.DTO.InstallmentModels;
using Ramz_Elktear.core.Entities.Installment;
using Ramz_Elktear.RepositoryLayer.Interfaces;

namespace Ramz_Elktear.BusinessLayer.Services
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JobDetails>> GetAllJobsAsync()
        {
            var jobs = await _unitOfWork.JobRepository.GetAllAsync();
            return jobs.Select(job => _mapper.Map<JobDetails>(job)).ToList();
        }

        public async Task<JobDetails> GetJobByIdAsync(string jobId)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(jobId);
            if (job == null) throw new ArgumentException("Job not found");

            return _mapper.Map<JobDetails>(job);
        }

        public async Task<JobDetails> AddJobAsync(AddJob jobDto)
        {
            var job = _mapper.Map<Job>(jobDto);
            await _unitOfWork.JobRepository.AddAsync(job);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<JobDetails>(job);
        }

        public async Task<bool> UpdateJobAsync(string jobId, JobDetails jobDto)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(jobId);
            if (job == null) throw new ArgumentException("Job not found");

            _mapper.Map(jobDto, job);
            _unitOfWork.JobRepository.Update(job);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the job: " + ex.Message, ex);
            }
        }

        public async Task<bool> DeleteJobAsync(string jobId)
        {
            var job = await _unitOfWork.JobRepository.GetByIdAsync(jobId);
            if (job == null) throw new ArgumentException("Job not found");

            _unitOfWork.JobRepository.Delete(job);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the job: " + ex.Message, ex);
            }
        }
    }
}
