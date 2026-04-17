using Microsoft.AspNetCore.Mvc;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Services;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;

namespace WSC.CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomersController(ICustomerService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CustomerResponseDto>>>> GetAll(CancellationToken ct)
        {
            var result = await _service.GetAllAsync(ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("add-customer")]
        public async Task<ActionResult<ApiResponse<int>>> CreateCustomer(CreateCustomerDto dto, CancellationToken ct)
        {
            var result = await _service.CreateCustomerAsync(dto, ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("deactivate-customer")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(int id, CancellationToken ct)
        {
            var result = await _service.DeleteCustomerAsync(id, ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> GetById(int id, CancellationToken ct)
        {
            var res = await _service.GetByIdAsync(id, ct);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);
        }
        [HttpPut("update-customer")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateAsync(UpdateCustomerDto dto, CancellationToken ct)
        {
            var res = await _service.UpdateCustomerAsync(dto, ct);
            if (!res.Success)
                return BadRequest(res);
            return Ok(res);

        }
        [HttpGet("paged-response")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CustomerResponseDto>>>> GetPagedCustomer([FromQuery] PaginationRequest request, CancellationToken ct)
        {
            var result = await _service.GetCustomersAsync(request, ct);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
