using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutOfOffice_API.Models.Domain;
using OutOfOffice_API.Models.DTO;
using OutOfOffice_API.Repositories.Interfaces;

namespace OutOfOffice_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestRepository repository;
        public LeaveRequestController(ILeaveRequestRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        [Authorize(Policy = "RequirePMorHR")]
        public async Task<IActionResult> GetAllLeaveRequests()
        {
            IEnumerable<LeaveRequest> requests = await repository.GetAllAsync();

            //Domain to DTO

            return Ok(requests);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireEmployeeRole")]
        public async Task<IActionResult> GetByEmployeeId(Guid id)
        {
            IEnumerable<LeaveRequest> allRequests = await repository.GetAllAsync();

            IEnumerable<LeaveRequest> requests = allRequests.Where(e => e.EmployeeId.Equals(id));
            //Domain to DTO

            return Ok(requests);
        }
        [HttpPost]
        [Authorize(Policy = "RequireEmployeeRole")]
        public async Task<IActionResult> CreateLeaveRequest(CreateLeaveRequestDto request)
        {
            LeaveRequest response = new LeaveRequest()
            {
                EmployeeId = request.EmployeeId,
                AbsenceReason = request.AbsenceReason,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Comment = request.Comment,
                Status = RequestStatus.New
            };
            var result = await repository.CreateAsync(response);
            if (result != null)
                return Ok(response);
            else
                return BadRequest();
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireEmployeeRole")]
        public async Task<IActionResult> EditLeaveRequest(Guid id, CreateLeaveRequestDto request)
        {
            LeaveRequest approvalRequest = new LeaveRequest()
            {
                Id = id,
                EmployeeId = request.EmployeeId,
                AbsenceReason = request.AbsenceReason,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Comment = request.Comment,
                Status = request.Status
            };
            var response = await repository.UpdateAsync(approvalRequest);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
