using Microsoft.AspNetCore.Mvc;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Common;

namespace WSC.CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomersController(ICustomerService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _service.GetAllAsync(ct);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
        [HttpPost("add-customer")]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto, CancellationToken ct)
        {
            var result = await _service.CreateCustomerAsync(dto, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("deactivate-customer")]
        public async Task<IActionResult> DeleteCustomer(int id, CancellationToken ct)
        {
            var result = await _service.DeleteCustomerAsync(id, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var res = await _service.GetByIdAsync(id, ct);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPut("update-customer")]
        public async Task<IActionResult> UpdateAsync(UpdateCustomerDto dto, CancellationToken ct)
        {
            var res = await _service.UpdateCustomerAsync(dto, ct);
            if (res.Success)
                return Ok(res);
            return BadRequest(res);

        }
        [HttpGet("paged-response")]
        public async Task<IActionResult> GetPagedCustomer([FromQuery] PaginationRequest request, CancellationToken ct)
        {
            var result = await _service.GetCustomersAsync(request, ct);
            return Ok(result);
        }
    }
}
