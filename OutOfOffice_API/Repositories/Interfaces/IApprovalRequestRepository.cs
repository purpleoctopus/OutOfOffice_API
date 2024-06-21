using OutOfOffice_API.Models.Domain;

namespace OutOfOffice_API.Repositories.Interfaces
{
    public interface IApprovalRequestRepository
    {
        Task<IEnumerable<ApprovalRequest>> GetAllAsync();
        Task<ApprovalRequest> UpdateAsync(ApprovalRequest approvalRequest);
    }
}
