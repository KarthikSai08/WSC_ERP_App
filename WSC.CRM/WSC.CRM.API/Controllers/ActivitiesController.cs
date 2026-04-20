using Microsoft.AspNetCore.Mvc;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _service;
        public ActivitiesController(IActivityService service)
        {
            _service = service;
        }
        [HttpGet("leadId/{leadId}")]
        public async Task<ActionResult<ApiResponse<ActivityResponseDto>>> GetActivitiesByLeadId(int leadId, CancellationToken ct)
        {
            var result = await _service.GetActivitiesByLeadIdAsync(leadId, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);

        }
        [HttpDelete("inactivate")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteActivity(int id, CancellationToken ct)
        {
            var result = await _service.DeleteActivityAsync(id, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpPost("create-activity")]
        public async Task<ActionResult<ApiResponse<int>>> CreateActivity([FromBody] CreateActivityDto dto, CancellationToken ct)
        {
            var result = await _service.CreateActivityAsync(dto, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpPut("update-activity")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateActivity([FromBody] UpdateActivityDto dto, CancellationToken ct)
        {
            var result = await _service.UpdateActivityAsync(dto, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet("activityId/{activityId}")]
        public async Task<ActionResult<ApiResponse<ActivityResponseDto>>> GetActivityById(int activityId, CancellationToken ct)
        {
            var result = await _service.GetActivityByIdAsync(activityId, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);

        }
        [HttpGet("all-activities")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ActivityResponseDto>>>> GetAllActivities(CancellationToken ct)
        {
            var result = await _service.GetAllActivitiesAsync(ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpGet("paged-activities")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ActivityResponseDto>>>> GetPagedActivities([FromQuery] PaginationRequest req, CancellationToken ct)
        {
            var result = await _service.GetPagedActivitiesAsync(req, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpPut("update-completed-at")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ActivityResponseDto>>>> UpdateCompletedAt(int actId, CancellationToken ct)
        {
            var result = await _service.UpdateCompletedAtAsync(actId, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }

    }
}
