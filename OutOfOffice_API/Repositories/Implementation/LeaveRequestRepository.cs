using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OutOfOffice_API.Data;
using OutOfOffice_API.Models.Domain;
using OutOfOffice_API.Repositories.Interfaces;

namespace OutOfOffice_API.Repositories.Implementation
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly AppDbContext dbcontext;
        public LeaveRequestRepository(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<LeaveRequest> CreateAsync(LeaveRequest request)
        {
            var difference = checkDate(request.StartDate, request.EndDate);
            if (difference > 0 && (await dbcontext.Employees.FirstAsync(x => x.Id == request.EmployeeId)).OOO_balance
                - difference >= 0)
            {
                await dbcontext.AddAsync(request);
                await dbcontext.SaveChangesAsync();
                return request;
            }
            return null;
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllAsync()
        {
            return await dbcontext.LeaveRequests.ToListAsync();
        }

        public async Task<LeaveRequest> GetByIdAsync(Guid id)
        {
            return await dbcontext.LeaveRequests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<LeaveRequest> UpdateAsync(LeaveRequest request)
        {
            var difference = checkDate(request.StartDate, request.EndDate);
            if (difference > 0 && (await dbcontext.Employees.FirstAsync(x => x.Id == request.EmployeeId)).OOO_balance
                - difference >= 0)
            {

            }
            else
            {
                return null;
            }
            LeaveRequest existingLeaveRequest = await dbcontext.LeaveRequests.FirstOrDefaultAsync(x => x.Id == request.Id);
            if(existingLeaveRequest != null && existingLeaveRequest.Status == RequestStatus.New)
            {
                dbcontext.Entry(existingLeaveRequest).CurrentValues.SetValues(request);
                await dbcontext.SaveChangesAsync();
                if(request.Status == RequestStatus.Submitted) {
                    ApprovalRequest approvalRequest = new ApprovalRequest()
                    {
                        Status = RequestStatus.New,
                        LeaveRequestId = request.Id
                    };
                    await dbcontext.ApprovalRequests.AddAsync(approvalRequest);
                    await dbcontext.SaveChangesAsync();
                }
                return request;
            }
            return null;
        }
        private int checkDate(DateOnly startDate, DateOnly endDate)
        {
            return (endDate.ToDateTime(new TimeOnly()) - startDate.ToDateTime(new TimeOnly())).Days;
        }
    }
}
