using Microsoft.EntityFrameworkCore;
using OutOfOffice_API.Data;
using OutOfOffice_API.Models.Domain;
using OutOfOffice_API.Repositories.Interfaces;

namespace OutOfOffice_API.Repositories.Implementation
{
    public class ApprovalRequestRepository : IApprovalRequestRepository
    {
        private readonly AppDbContext dbcontext;
        public ApprovalRequestRepository(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<IEnumerable<ApprovalRequest>> GetAllAsync()
        {
            return await dbcontext.ApprovalRequests.ToListAsync();
        }

        public async Task<ApprovalRequest> UpdateAsync(ApprovalRequest approvalRequest)
        {
            ApprovalRequest existingApprovalRequest = await dbcontext.ApprovalRequests.FirstOrDefaultAsync(x => x.Id == approvalRequest.Id);
            if (existingApprovalRequest != null && existingApprovalRequest.Status == RequestStatus.New && 
                (await dbcontext.Employees.FirstOrDefaultAsync(x => x.Id == approvalRequest.ApproverId))?.Position != Position.Employee)
            {
                var newRequest = existingApprovalRequest;
                newRequest.ApproverId = approvalRequest.ApproverId;
                newRequest.Status = approvalRequest.Status;
                newRequest.Comment = approvalRequest.Comment;
                dbcontext.Entry(existingApprovalRequest).CurrentValues.SetValues(newRequest);
                var leaveRequest = await dbcontext.LeaveRequests.FirstOrDefaultAsync(x => x.Id == approvalRequest.LeaveRequestId);

                LeaveRequest newLeaveRequest = new LeaveRequest() { 
                    Id = leaveRequest.Id,
                    EmployeeId = leaveRequest.EmployeeId,
                    AbsenceReason = leaveRequest.AbsenceReason,
                    StartDate = leaveRequest.StartDate,
                    EndDate = leaveRequest.EndDate,
                    Comment = leaveRequest.Comment,
                    Status = approvalRequest.Status
                };
                dbcontext.Entry(leaveRequest).CurrentValues.SetValues(newLeaveRequest);
                if(approvalRequest.Status == RequestStatus.Approved)
                {
                    Employee employee = await dbcontext.Employees.FirstAsync(x => x.Id == leaveRequest.EmployeeId);
                    Employee newEmployee = new Employee()
                    {
                        Id = employee.Id,
                        FullName = employee.FullName,
                        Subdivizion = employee.Subdivizion,
                        Position = employee.Position,
                        Status = employee.Status,
                        PeoplePartnerId = employee.PeoplePartnerId,
                        OOO_balance = employee.OOO_balance - checkDate(leaveRequest.StartDate, leaveRequest.EndDate),
                        Photo = employee.Photo
                    };
                    dbcontext.Entry(employee).CurrentValues.SetValues(newEmployee);
                }
                await dbcontext.SaveChangesAsync();
                return newRequest;
            }
            return null;
        }
        private int checkDate(DateOnly startDate, DateOnly endDate)
        {
            return (endDate.ToDateTime(new TimeOnly()) - startDate.ToDateTime(new TimeOnly())).Days;
        }
    }
}
