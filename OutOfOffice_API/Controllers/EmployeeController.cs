using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice_API.Models.Domain;
using OutOfOffice_API.Repositories.Interfaces;

namespace OutOfOffice_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequirePMorHR")]
    public class EmployeeController : ControllerBase
    {
        public readonly IEmployeeRepository repository;
        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            IEnumerable<Employee> employees = await repository.GetAllAsync();

            //Domain to DTO
            /*List<EmployeeDto> response = new List<EmployeeDto>();
            foreach (Employee employee in employees)
            {
                response.Add(new EmployeeDto
                {
                    Id = film.Id,
                    Name = film.Name,
                    Description = film.Description,
                    CategoryId = film.CategoryId,
                    Age = film.Age,
                    Image = null
                });
            }*/

            return Ok(employees);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditEmployee(Guid id, CreateEmployeeDto request)
        {
            Employee employee = new Employee()
            {
                Id = id,
                FullName = request.FullName,
                Subdivizion = request.Subdivizion,
                Position = request.Position,
                Status = request.Status,
                PeoplePartnerId = request.PeoplePartnerId,
                OOO_balance = request.OOO_balance,
                Photo = request.Photo
            };
            Employee response = await repository.UpdateAsync(employee);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
