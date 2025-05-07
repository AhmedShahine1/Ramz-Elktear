using Ramz_Elktear.core.DTO.InstallmentRequestModels;
using Ramz_Elktear.core.Helper;

namespace Ramz_Elktear.BusinessLayer.Interfaces
{
    public interface IInstallmentRequestService
    {
        Task<IEnumerable<DetailsInstallmentRequest>> FilterInstallmentRequests(InstallmentRequestFilter filter);
        Task<DetailsInstallmentRequest> AddInstallmentRequestWithUserCheck(AddInstallmentRequest model);
        Task<DetailsInstallmentRequest> UpdateInstallmentRequest(UpdateInstallmentRequest model);
        Task<bool> DeleteInstallmentRequest(string id);
        Task<DetailsInstallmentRequest> CheckInstallmentWithoutRequest(AddInstallmentRequest model);
        Task<bool> UpdateInstallmentRequestStatus(string id, InstallmentStatus newStatus);
        Task<DataInstallmentRequest> DataInstallmentRequest();
    }
}
