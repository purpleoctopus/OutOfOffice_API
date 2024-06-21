using Microsoft.EntityFrameworkCore;
using OutOfOffice_API.Data;
using OutOfOffice_API.Models.Domain;
using OutOfOffice_API.Repositories.Interfaces;

namespace OutOfOffice_API.Repositories.Implementation
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext dbcontext;
        public ProjectRepository(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public async Task<Project> CreateAsync(Project request)
        {
            await dbcontext.AddAsync(request);
            await dbcontext.SaveChangesAsync();
            return request;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await dbcontext.Projects.ToListAsync();
        }

        public async Task<Project> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        async public Task<Project> UpdateAsync(Project employee)
        {
            Project existingProject = await dbcontext.Projects.FirstOrDefaultAsync(x => x.Id == employee.Id);
            if (existingProject != null)
            {
                dbcontext.Entry(existingProject).CurrentValues.SetValues(employee);
                await dbcontext.SaveChangesAsync();
                return employee;
            }

            return null;
        }
    }
}
