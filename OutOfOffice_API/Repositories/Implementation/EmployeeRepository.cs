using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OutOfOffice_API.Data;
using OutOfOffice_API.Models.Domain;
using OutOfOffice_API.Repositories.Interfaces;

namespace OutOfOffice_API.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext dbcontext;
        public EmployeeRepository(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<Employee> CreateAsync(Employee employee)
        {
            await dbcontext.AddAsync(employee);
            await dbcontext.SaveChangesAsync();
            return employee;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await dbcontext.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(Guid id)
        {
            return await dbcontext.Employees.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            Employee existingEmployee = await dbcontext.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);
            if (existingEmployee != null)
            {
                dbcontext.Entry(existingEmployee).CurrentValues.SetValues(employee);
                await dbcontext.SaveChangesAsync();
                return employee;
            }

            return null;
        }
        public async Task<Employee> DeleteAsync(Employee employee)
        {
            dbcontext.Employees.Remove(employee);
            await dbcontext.SaveChangesAsync();
            return null;
        }
    }
}
