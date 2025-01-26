using Microsoft.AspNetCore.Http;
using Ramz_Elktear.core.DTO.BranchModels;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchDetails>> GetAllBranchesAsync();
        Task<BranchDetails> GetBranchByIdAsync(string branchId);
        Task<BranchDetails> AddBranchAsync(AddBranch branchDto);
        Task<bool> UpdateBranchAsync(BranchDetails branchDto, IFormFile newImage = null);
        Task<bool> DeleteBranchAsync(string branchId);
    }
}
