using OutOfOffice_API.Models.Domain;

namespace OutOfOffice_API.Repositories.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> CreateAsync(Project employee);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(Guid id);
        Task<Project> UpdateAsync(Project employee);
    }
}
