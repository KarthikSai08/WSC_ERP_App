using Microsoft.AspNetCore.Mvc;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _service;

        public LeadsController(ILeadService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<LeadResponseDto>>>> GetAllLeads(CancellationToken ct)
        {
            var leads = await _service.GetAllLeadsAsync(ct);
            return Ok(leads);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LeadResponseDto>>> GetLeadById(int id, CancellationToken ct)
        {
            var lead = await _service.GetLeadByIdAsync(id, ct);
            if (lead == null) return NotFound();
            return Ok(lead);
        }
        [HttpPost("create-lead")]
        public async Task<ActionResult<ApiResponse<int>>> CreateLead([FromBody] CreateLeadDto request, CancellationToken ct)
        {
            var newLeadId = await _service.CreateLeadAsync(request, ct);
            return CreatedAtAction(nameof(GetLeadById), new { id = newLeadId }, null);
        }
        [HttpPut("update-lead")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateLead([FromBody] UpdateLeadDto request, CancellationToken ct)
        {
            var success = await _service.UpdateLeadAsync(request, ct);
            if (success is null) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteLead(int id, CancellationToken ct)
        {
            var success = await _service.DeleteLeadAsync(id, ct);
            if (success is null) return NotFound();
            return NoContent();
        }
        [HttpGet("paged-response")]
        public async Task<ActionResult<ApiResponse<IEnumerable<LeadResponseDto>>>> GetLeads([FromQuery] PaginationRequest request, CancellationToken ct)
        {
            var result = await _service.GetLeadsAsync(request, ct);
            return Ok(result);
        }
    }
}