using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice_API.Models.Domain
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [EnumDataType(typeof(ProjectType))]
        public ProjectType ProjectType { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }

        [Required]
        public Guid ProjectManagerId { get; set; }

        [ForeignKey("ProjectManagerId")]
        public Employee ProjectManager { get; set; }

        public string? Comment { get; set; }

        [Required]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; } = Status.Active;
    }

    public enum ProjectType
    {
        Internal,
        External,
        Research,
        Development
    }

    public enum Status
    {
        Active,
        Inactive
    }
}
