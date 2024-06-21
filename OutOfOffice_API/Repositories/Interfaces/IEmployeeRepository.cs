using OutOfOffice_API.Models.Domain;

namespace OutOfOffice_API.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Employee> CreateAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(Guid id);
        Task<Employee> UpdateAsync(Employee employee);
        Task<Employee> DeleteAsync(Employee employee);
    }
}
