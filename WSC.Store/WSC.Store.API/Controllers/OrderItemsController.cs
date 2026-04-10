using Microsoft.AspNetCore.Mvc;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.ServiceInterfaces;

namespace WSC.Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemsService _service;
        public OrderItemsController(IOrderItemsService service) => _service = service;

        [HttpGet("order-items")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderItemResponseDto>>>> GetAll(CancellationToken ct)
        {
            var result = await _service.GetAllOrderItemsAsync(ct);
            if (result.Success)
                return Ok(result);
            return NotFound(result);

        }
        [HttpGet("order-item/{oiId}")]
        public async Task<ActionResult<ApiResponse<OrderItemResponseDto>>> GetById([FromQuery] int oiId, CancellationToken ct)
        {
            var result = await _service.GetItemByIdAsync(oiId, ct);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }

        [HttpPost("create-order-item")]
        public async Task<ActionResult<ApiResponse<int>>> CreateOrderItem([FromBody] CreateItemsDto items, CancellationToken ct)
        {
            var result = await _service.CreateOrderItemAsync(items, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("update-order-items")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderItem([FromBody] UpdateItemsDto items, CancellationToken ct)
        {
            var result = await _service.UpdateOrderItemAsync(items, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete-item")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteItem(int oiId, CancellationToken ct)
        {
            var result = await _service.DeleteOrderItemAsync(oiId, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
