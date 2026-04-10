using Microsoft.AspNetCore.Mvc;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.StoreLayer;
using WSC.Store.Application.Dtos;
using WSC.Store.Application.Interfaces.ServiceInterfaces;

namespace WSC.Store.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;
        public InventoryController(IInventoryService service) => _service = service;

        [HttpGet("inventory-records")]
        public async Task<ActionResult<ApiResponse<IEnumerable<InventoryResponseDto>>>> GetAllInventoryRecords(CancellationToken ct)
        {
            var result = await _service.GetAllInventoryRecordsAsync(ct);
            return Ok(result);
        }
        [HttpGet("inventory-record/{id}")]
        public async Task<ActionResult<ApiResponse<InventoryResponseDto>>> GetInventoryRecordById(int id, CancellationToken ct)
        {
            var result = await _service.GetInventoryRecordByIdAsync(id, ct);
            if (result.Data == null)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPost("create-inventory-record")]
        public async Task<ActionResult<ApiResponse<int>>> CreateInventoryRecord([FromBody] CreateInventoryRecordDto record, CancellationToken ct)
        {
            var result = await _service.CreateInventoryRecordAsync(record, ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("delete-inventory-record/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteInventoryRecord(int id, CancellationToken ct)
        {
            var result = await _service.DeleteInventoryRecordAsync(id, ct);
            if (!result.Success)
                return NotFound(result);
            return Ok(result);
        }

        [HttpPut("update-stock/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateStock(int id, [FromQuery] int quantity, CancellationToken ct)
        {
            var result = await _service.UpdateStockAsync(id, quantity, ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
