using OutOfOffice_API.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OutOfOffice_API.Models.DTO
{
    public class CreateProjectDto
    {
        public ProjectType ProjectType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public Guid ProjectManagerId { get; set; }
        public string? Comment { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
