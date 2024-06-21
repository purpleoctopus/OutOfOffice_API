using OutOfOffice_API.Models.Domain;
using System.Diagnostics.CodeAnalysis;

namespace OutOfOffice_API.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public string FullName { get; set; }
        public Subdivizion Subdivizion { get; set; }
        public Position Position { get; set; }
        public Status Status { get; set; } = Status.Active;
        public string? PeoplePartnerId { get; set; }
        public float OOO_balance { get; set; }
        public byte[]? Photo { get; set; }
    }
}
