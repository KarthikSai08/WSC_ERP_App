using Microsoft.AspNetCore.Mvc;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

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
        public async Task<ActionResult<ApiResponse<IEnumerable<OpportunityResponseDto>>>> GetAllOpportunitiesAsync(CancellationToken ct)
        {
            var result = await _service.GetAllOpportunitiesAsync(ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse<int>>> CreateOpportunityAsync([FromBody] CreateOpportunityDto dto, CancellationToken ct)
        {
            var result = await _service.CreateOpportunityAsync(dto, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteOpportunityAsync(int id, CancellationToken ct)
        {
            var result = await _service.DeleteOpportunityAsync(id, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpGet("GetByCustomer/{cxId}")]
        public async Task<ActionResult<ApiResponse<OpportunityResponseDto>>> GetOpportunitiesByCustomerIdAsync(int cxId, CancellationToken ct)
        {
            var result = await _service.GetOpportunitiesByCustomerIdAsync(cxId, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpPost("UpdateClosedAt/{oppId}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateClosedAtAsync(int oppId, CancellationToken ct)
        {
            var result = await _service.UpdateClosedAtAsync(oppId, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpGet("GetPaged")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OpportunityResponseDto>>>> GetPagedOpportunitiesAsync([FromQuery] PaginationRequest request, CancellationToken ct)
        {
            var result = await _service.GetPagedOpportunitiesAsync(request, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOpportunityAsync([FromBody] UpdateOpportunityDto dto, CancellationToken ct)
        {
            var result = await _service.UpdateOpportunityAsync(dto, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<ApiResponse<OpportunityResponseDto>>> GetOpportunityByIdAsync(int id, CancellationToken ct)
        {
            var result = await _service.GetOpportunityByIdAsync(id, ct);
            if (!result.Success)
                return BadRequest();
            return Ok(result);
        }

    }
}
