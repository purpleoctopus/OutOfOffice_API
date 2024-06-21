using OutOfOffice_API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice_API.Models.DTO
{
    public class CreateLeaveRequestDto
    {
        public Guid EmployeeId { get; set; }
        public AbsenceReason AbsenceReason { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Comment { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.New;
    }
}
