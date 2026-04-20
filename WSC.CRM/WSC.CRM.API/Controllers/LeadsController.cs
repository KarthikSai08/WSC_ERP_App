using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class LeadsController : ControllerBase
    {
        private readonly ILeadService _service;

        public LeadsController(ILeadService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<LeadResponseDto>>>> GetAllLeads(CancellationToken ct)
        {
            var result = await _service.GetAllLeadsAsync(ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LeadResponseDto>>> GetLeadById(int id, CancellationToken ct)
        {
            var result = await _service.GetLeadByIdAsync(id, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
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
            var result = await _service.UpdateLeadAsync(request, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteLead(int id, CancellationToken ct)
        {
            var result = await _service.DeleteLeadAsync(id, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpGet("paged-response")]
        public async Task<ActionResult<ApiResponse<IEnumerable<LeadResponseDto>>>> GetLeads([FromQuery] PaginationRequest request, CancellationToken ct)
        {
            var result = await _service.GetLeadsAsync(request, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
    }
}