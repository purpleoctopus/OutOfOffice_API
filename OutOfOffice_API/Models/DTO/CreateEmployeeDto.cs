using System.Reflection.Metadata;

namespace OutOfOffice_API.Models.Domain
{
    public class CreateEmployeeDto
    {
        public string FullName { get; set; }
        public Subdivizion Subdivizion { get; set; }
        public Position Position { get; set; }
        public Status Status { get; set; }
        public Guid? PeoplePartnerId { get; set; }
        public float OOO_balance { get; set; }
        public byte[]? Photo { get; set; }
    }
}
