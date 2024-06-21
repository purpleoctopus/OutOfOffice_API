using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice_API.Models.Domain;
using OutOfOffice_API.Models.DTO;
using OutOfOffice_API.Repositories.Interfaces;

namespace OutOfOffice_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequirePMorHR")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository repository;
        public ProjectController(IProjectRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            IEnumerable<Project> response = await repository.GetAllAsync();

            //Domain to DTO

            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto request)
        {
            Project response = new Project()
            {
                ProjectType = request.ProjectType,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ProjectManagerId = request.ProjectManagerId,
                Comment = request.Comment,
                Status = request.Status
            };
            await repository.CreateAsync(response);
            return Ok(response);
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "RequirePMRole")]
        public async Task<IActionResult> EditProject(Guid id, CreateProjectDto request)
        {
            Project project = new Project()
            {
                Id = id,
                ProjectType = request.ProjectType,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ProjectManagerId = request.ProjectManagerId,
                Comment = request.Comment,
                Status = request.Status
            };
            Project response = await repository.UpdateAsync(project);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
