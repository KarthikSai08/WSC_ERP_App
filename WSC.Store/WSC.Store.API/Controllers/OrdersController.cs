using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.ServiceInterfaces;

namespace WSC.Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet("all-orders-summary")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _service.GetAllOrdersAsync(ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("order/{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(id, ct);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto dto, CancellationToken ct)
        {
            var result = await _service.CreateOrderAsync(dto, ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("delete-order/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _service.DeleteOrderAsync(id, ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("update-order/{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDto dto, CancellationToken ct)
        {
            var result = await _service.UpdateOrderAsync(dto, ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
