using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OutOfOffice_API.Models;
using OutOfOffice_API.Models.Domain;

namespace OutOfOffice_API.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}