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
    public class ApprovalRequestController : ControllerBase
    {
        private readonly IApprovalRequestRepository repository;
        public ApprovalRequestController(IApprovalRequestRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetApprovalRequests()
        {
            IEnumerable<ApprovalRequest> approvalRequests = await repository.GetAllAsync();

            //

            return Ok(approvalRequests);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> EditApprovalRequest(Guid id, CreateApprovalRequestDto request)
        {
            ApprovalRequest approvalRequest = new ApprovalRequest()
            {
                Id = id,
                ApproverId = request.ApproverId,
                LeaveRequestId = request.LeaveRequestId,
                Status = request.Status,
                Comment = request.Comment
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
