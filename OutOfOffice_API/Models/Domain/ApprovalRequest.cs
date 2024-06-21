using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice_API.Models.Domain
{
    public class ApprovalRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? ApproverId { get; set; }

        [ForeignKey("ApproverId")]
        public Employee? Approver { get; set; }

        [Required]
        public Guid LeaveRequestId { get; set; }

        [ForeignKey("LeaveRequestId")]
        public LeaveRequest LeaveRequest { get; set; }

        [Required]
        [EnumDataType(typeof(RequestStatus))]
        public RequestStatus Status { get; set; } = RequestStatus.New;

        public string? Comment { get; set; }
    }

    public enum RequestStatus
    {
        New,
        Submitted,
        Canceled,
        Approved,
        Rejected
    }
}
