using Microsoft.AspNetCore.Mvc;
using WSC.Delivery.Application.Dtos;
using WSC.Delivery.Application.Interfaces.ServiceInterfaces;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.DeliveryLayer;

namespace WSC.Delivery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class DeliveryTrackingController : ControllerBase
    {
        private readonly IDeliveryTrackingService _deliveryTrackingService;

        public DeliveryTrackingController(IDeliveryTrackingService deliveryTrackingService)
        {
            _deliveryTrackingService = deliveryTrackingService ?? throw new ArgumentNullException(nameof(deliveryTrackingService));
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiResponse<int>>> CreateTrackingRecord(
            [FromBody] CreateDeliveryTrackingDto dto,
            CancellationToken ct)
        {
            var result = await _deliveryTrackingService.CreateTrackingRecordAsync(dto, ct);
            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetTrackingById), new { trackingId = result.Data }, result);
        }

        [HttpGet("{trackingId}")]
        public async Task<ActionResult<ApiResponse<DeliveryTrackingResponseDto>>> GetTrackingById(
            int trackingId,
            CancellationToken ct)
        {
            var result = await _deliveryTrackingService.GetTrackingByIdAsync(trackingId, ct);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("delivery/{deliveryId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>>> GetTrackingByDeliveryId(
            int deliveryId,
            CancellationToken ct)
        {
            var result = await _deliveryTrackingService.GetTrackingByDeliveryIdAsync(deliveryId, ct);
            return Ok(result);
        }

        [HttpGet("delivery/{deliveryId}/latest")]
        public async Task<ActionResult<ApiResponse<IEnumerable<DeliveryTrackingResponseDto>>>> GetLatestTracking(
            int deliveryId,
            [FromQuery] int count = 5,
            CancellationToken ct = default)
        {
            var result = await _deliveryTrackingService.GetLatestTrackingAsync(deliveryId, count, ct);
            return Ok(result);
        }
    }
}
