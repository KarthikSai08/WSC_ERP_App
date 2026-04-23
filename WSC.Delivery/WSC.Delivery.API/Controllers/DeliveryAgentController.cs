using Microsoft.AspNetCore.Mvc;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class DeliveryAgentController : ControllerBase
    {
        private readonly IDeliveryAgentService _deliveryAgentService;

        public DeliveryAgentController(IDeliveryAgentService deliveryAgentService)
        {
            _deliveryAgentService = deliveryAgentService ?? throw new ArgumentNullException(nameof(deliveryAgentService));
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<int>>> CreateDeliveryAgent(
            [FromBody] CreateDeliveryAgentDto dto,
            CancellationToken ct)
        {
            var result = await _deliveryAgentService.CreateDeliveryAgentAsync(dto, ct);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetDeliveryAgentById), new { agentId = result.Data }, result);
        }

        [HttpGet("{agentId}")]
        public async Task<ActionResult<ApiResponse<DeliveryAgentResponseDto>>> GetDeliveryAgentById(
            int agentId,
            CancellationToken ct)
        {
            var result = await _deliveryAgentService.GetAgentByIdAsync(agentId, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("all-agents")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DeliveryAgentResponseDto>>>> GetAllDeliveryAgents(
            CancellationToken ct)
        {
            var result = await _deliveryAgentService.GetAllAgentsAsync(ct);
            return Ok(result);
        }

        [HttpGet("available")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DeliveryAgentResponseDto>>>> GetAvailableAgents(
            CancellationToken ct)
        {
            var result = await _deliveryAgentService.GetAvailableAgentsAsync(ct);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateDeliveryAgent(
            [FromBody] UpdateDeliveryAgentDto dto,
            CancellationToken ct)
        {
            var result = await _deliveryAgentService.UpdateDeliveryAgentAsync(dto, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{agentId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteDeliveryAgent(
            int agentId,
            CancellationToken ct)
        {
            var result = await _deliveryAgentService.DeleteAgentAsync(agentId, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
