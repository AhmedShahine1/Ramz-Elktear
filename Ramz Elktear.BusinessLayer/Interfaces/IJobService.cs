using Ramz_Elktear.core.DTO.InstallmentModels;

public interface IJobService
{
    Task<IEnumerable<JobDetails>> GetAllJobsAsync();
    Task<JobDetails> GetJobByIdAsync(string jobId);
    Task<JobDetails> AddJobAsync(AddJob jobDto);
    Task<bool> UpdateJobAsync(UpdateJob jobDto);
    Task<bool> DeleteJobAsync(string jobId);
}