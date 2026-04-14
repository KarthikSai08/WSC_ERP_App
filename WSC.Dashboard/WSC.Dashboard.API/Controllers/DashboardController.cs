using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSC.Dashboard.Application.Interfaces.ServiceInterfaces;

namespace WSC.Dashboard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService service) => _service = service;

        [HttpGet("customer/{cxId}")]
        public async Task<IActionResult> GetCustomerDashboard(int cxId, CancellationToken ct)
        {
            var result = await _service.GetCustomerDashBoard(cxId, ct);
            if (result.Success)
                return Ok(result);
            return NotFound(result);
        }
    } 
}
