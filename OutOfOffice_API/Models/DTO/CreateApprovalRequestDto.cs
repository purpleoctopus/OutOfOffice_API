using OutOfOffice_API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice_API.Models.DTO
{
    public class CreateApprovalRequestDto
    {
        public Guid? ApproverId { get; set; }
        public Guid LeaveRequestId { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.New;
        public string? Comment { get; set; }
    }
}
