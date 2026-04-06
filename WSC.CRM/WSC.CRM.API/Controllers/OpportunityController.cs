using Microsoft.AspNetCore.Mvc;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Common;

namespace WSC.CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpportunityController : ControllerBase
    {
        private readonly IOpportunityService _service;
        public OpportunityController(IOpportunityService service)
        {
            _service = service;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllOpportunitiesAsync(CancellationToken ct)
        {
            var result = await _service.GetAllOpportunitiesAsync(ct);
            if (result is not null)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateOpportunityAsync([FromBody] CreateOpportunityDto dto, CancellationToken ct)
        {
            var result = await _service.CreateOpportunityAsync(dto, ct);
            if (result is not null)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOpportunityAsync(int id, CancellationToken ct)
        {
            var result = await _service.DeleteOpportunityAsync(id, ct);
            if (result is not null)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetByCustomer/{cxId}")]
        public async Task<IActionResult> GetOpportunitiesByCustomerIdAsync(int cxId, CancellationToken ct)
        {
            var result = await _service.GetOpportunitiesByCustomerIdAsync(cxId, ct);
            if (result is not null)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("UpdateClosedAt/{oppId}")]
        public async Task<IActionResult> UpdateClosedAtAsync(int oppId, CancellationToken ct)
        {
            var result = await _service.UpdateClosedAtAsync(oppId, ct);
            if (result is not null)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetPaged")]
        public async Task<IActionResult> GetPagedOpportunitiesAsync([FromQuery] PaginationRequest request, CancellationToken ct)
        {
            var result = await _service.GetPagedOpportunitiesAsync(request, ct);
            if (result is not null)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateOpportunityAsync([FromBody] UpdateOpportunityDto dto, CancellationToken ct)
        {
            var result = await _service.UpdateOpportunityAsync(dto, ct);
            if (result is not null)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetOpportunityByIdAsync(int id, CancellationToken ct)
        {
            var result = await _service.GetOpportunityByIdAsync(id, ct);
            if (result is not null)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
