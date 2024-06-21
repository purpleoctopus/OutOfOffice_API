using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace OutOfOffice_API.Models.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public Subdivizion Subdivizion { get; set; }
        public Position Position { get; set; }
        public Status Status { get; set; } = Status.Active;
        [AllowNull]
        public Guid? PeoplePartnerId { get; set; }
        [AllowNull]
        public Employee? PeoplePartner { get; set; }
        public float OOO_balance { get; set; }
        public byte[]? Photo { get; set; }
    }
    public enum Subdivizion
    {
        HR,
        IT,
        Finance
    }

    public enum Position
    {
        Employee,
        HRmanager,
        ProjectManager
    }
}
