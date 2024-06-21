using OutOfOffice_API.Models.Domain;

namespace OutOfOffice_API.Repositories.Interfaces
{
    public interface ILeaveRequestRepository
    {
        Task<LeaveRequest> CreateAsync(LeaveRequest leaveRequest);
        Task<IEnumerable<LeaveRequest>> GetAllAsync();
        Task<LeaveRequest> GetByIdAsync(Guid id);
        Task<LeaveRequest> UpdateAsync(LeaveRequest leaveRequest);
    }
}
