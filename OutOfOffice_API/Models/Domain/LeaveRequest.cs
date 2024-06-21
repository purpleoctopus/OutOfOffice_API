using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice_API.Models.Domain
{
    public class LeaveRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required]
        [EnumDataType(typeof(AbsenceReason))]
        public AbsenceReason AbsenceReason { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }

        public string? Comment { get; set; }

        [Required]
        [EnumDataType(typeof(RequestStatus))]
        public RequestStatus Status { get; set; } = RequestStatus.New;
    }

    public enum AbsenceReason
    {
        SickLeave,
        Vacation,
        PersonalLeave,
        Other
    }
}
