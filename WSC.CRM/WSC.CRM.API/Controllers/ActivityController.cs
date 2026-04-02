using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Common;

namespace WSC.CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _service;
        public ActivityController(IActivityService service)
        {
            _service = service;
        }
        [HttpGet("leadId-{leadId}")]
        public async Task<IActionResult> GetActivitiesByLeadId(int leadId, CancellationToken ct)
        {
            var result = await _service.GetActivitiesByLeadIdAsync(leadId, ct);
            return Ok(result);
        }
        [HttpDelete("inactivate")]
        public async Task<IActionResult> DeleteActivity(int id, CancellationToken ct)
        {
            var result = await _service.DeleteActivityAsync(id, ct);
            return Ok(result);
        }
        [HttpPost("create-activity")]
        public async Task<IActionResult> CreateActivity([FromBody] CreateActivityDto dto, CancellationToken ct)
        {
            var result = await _service.CreateActivityAsync(dto, ct);
            return Ok(result);
        }
        [HttpPut("update-activity")]
        public async Task<IActionResult> UpdateActivity([FromBody] UpdateActivityDto dto, CancellationToken ct)
        {
            var result = await _service.UpdateActivityAsync(dto, ct);
            return Ok(result);
        }

        [HttpGet("activityId-{activityId}")]
        public async Task<IActionResult> GetActivityById(int activityId, CancellationToken ct)
        {
            var result = await _service.GetActivityByIdAsync(activityId, ct);
            return Ok(result);

        }
        [HttpGet("all-activities")]
        public async Task<IActionResult> GetAllActivities(CancellationToken ct)
        {
            var result = await _service.GetAllActivitiesAsync(ct);
            return Ok(result);
        }
        [HttpGet("paged-activities")]
        public async Task<IActionResult> GetPagedActivities([FromQuery] PaginationRequest req, CancellationToken ct)
        {
            var result = await _service.GetPagedActivitiesAsync(req, ct);
            return Ok(result);
        }
        [HttpPut("update-completed-at")]
        public async Task<IActionResult> UpdateCompletedAt(int actId, CancellationToken ct)
        {
            var result = await _service.UpdateCompletedAtAsync(actId, ct);
            return Ok(result);
        }

    }
}
