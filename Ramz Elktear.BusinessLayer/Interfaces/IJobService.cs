using Ramz_Elktear.core.DTO.InstallmentModels;

public interface IJobService
{
    Task<IEnumerable<JobDTO>> GetAllAsync();
    Task<JobDTO> GetByIdAsync(string id);
    Task<bool> AddAsync(AddJobDTO dto);
    Task<bool> UpdateAsync(JobDTO dto);
    Task<bool> DeleteAsync(string id);
}
