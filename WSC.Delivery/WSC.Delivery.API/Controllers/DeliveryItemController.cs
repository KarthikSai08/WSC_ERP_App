using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WSC.Delivery.API.RateLimiting;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting(RateLimitingPolicy.DefaultPolicy)]
    public sealed class DeliveryItemController : ControllerBase
    {
        private readonly IDeliveryItemService _deliveryItemService;

        public DeliveryItemController(IDeliveryItemService deliveryItemService)
        {
            _deliveryItemService = deliveryItemService ?? throw new ArgumentNullException(nameof(deliveryItemService));
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<int>>> CreateDeliveryItem(
            [FromBody] CreateDeliveryItemDto dto,
            CancellationToken ct)
        {
            var result = await _deliveryItemService.CreateDeliveryItemAsync(dto, ct);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetDeliveryItemById), new { itemId = result.Data }, result);
        }

        [HttpGet("{itemId}")]
        public async Task<ActionResult<ApiResponse<DeliveryItemResponseDto>>> GetDeliveryItemById(
            int itemId,
            CancellationToken ct)
        {
            var result = await _deliveryItemService.GetDeliveryItemByIdAsync(itemId, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("delivery/{deliveryId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DeliveryItemResponseDto>>>> GetDeliveryItemsByDeliveryId(
            int deliveryId,
            CancellationToken ct)
        {
            var result = await _deliveryItemService.GetDeliveryItemsByDeliveryIdAsync(deliveryId, ct);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateDeliveryItem(
            [FromBody] UpdateDeliveryItemDto dto,
            CancellationToken ct)
        {
            var result = await _deliveryItemService.UpdateDeliveryItemAsync(dto, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{itemId}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteDeliveryItem(
            int itemId,
            CancellationToken ct)
        {
            var result = await _deliveryItemService.DeleteDeliveryItemAsync(itemId, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
