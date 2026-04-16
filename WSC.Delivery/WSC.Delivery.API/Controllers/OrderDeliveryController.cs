using Microsoft.AspNetCore.Mvc;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderDeliveryController : ControllerBase
    {
        private readonly IOrderDeliveryService _orderDeliveryService;

        public OrderDeliveryController(IOrderDeliveryService orderDeliveryService)
        {
            _orderDeliveryService = orderDeliveryService ?? throw new ArgumentNullException(nameof(orderDeliveryService));
        }

        [HttpPost("create-orderDelivery")]
        public async Task<ActionResult<ApiResponse<int>>> CreateOrderDelivery(
            [FromBody] CreateOrderDeliveryDto dto,
            CancellationToken ct)
        {
            var result = await _orderDeliveryService.CreateOrderDeliveryAsync(dto, ct);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetDeliveryById), new { deliveryId = result.Data }, result);
        }

        [HttpGet("{deliveryId}")]
        public async Task<ActionResult<ApiResponse<OrderDeliveryResponseDto>>> GetDeliveryById(
            int deliveryId,
            CancellationToken ct)
        {
            var result = await _orderDeliveryService.GetDeliveryByIdAsync(deliveryId, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("all-orderDelivery")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>>> GetAllDeliveries(
            CancellationToken ct)
        {
            var result = await _orderDeliveryService.GetAllDeliveriesAsync(ct);
            return Ok(result);
        }

        [HttpPut("update-orderDelivery")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderDelivery(
            [FromBody] UpdateOrderDeliveryDto dto,
            CancellationToken ct)
        {
            var result = await _orderDeliveryService.UpdateOrderDeliveryAsync(dto, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("delete/{deliveryId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteDelivery(
            int deliveryId,
            CancellationToken ct)
        {
            var result = await _orderDeliveryService.DeleteDeliveryAsync(deliveryId, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>>> GetDeliveriesByStatus(
            string status,
            CancellationToken ct)
        {
            var result = await _orderDeliveryService.GetDeliveriesByStatusAsync(status, ct);
            return Ok(result);
        }

        [HttpGet("agent/{agentId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderDeliveryResponseDto>>>> GetDeliveriesByAgentId(
            int agentId,
            CancellationToken ct)
        {
            var result = await _orderDeliveryService.GetDeliveriesByAgentIdAsync(agentId, ct);
            return Ok(result);
        }
    }
}
