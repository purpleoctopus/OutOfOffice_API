using Microsoft.AspNetCore.Identity;
using OutOfOffice_API.Models.Domain;

namespace OutOfOffice_API.Models
{

    public class ApplicationUser : IdentityUser
    {
        public Employee Employee { get; set; }
        public Guid EmployeeID {  get; set; }
    }
}
