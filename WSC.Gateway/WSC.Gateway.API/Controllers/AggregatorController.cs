using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WSC.Gateway.API.RateLimiting;
using WSC.Gateway.Application.Dtos.AggregatorDtos;
using WSC.Gateway.Application.Interfaces;
using WSC.Shared.Contracts.Common;

namespace WSC.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AggregatorController : ControllerBase
    {
        private readonly IAggregatorService _service;
        public AggregatorController(IAggregatorService service) => _service = service;

        [HttpGet("summary")]
        [EnableRateLimiting(RateLimitingPolicy.DefaultPolicy)]
        public async Task<ActionResult<ApiResponse<AppSummaryDto>>> GetSummaryAsync(CancellationToken ct)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Viewer";
            var name = User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

            var result = await _service.GetAppSummaryAsync(userId, role, ct);

            if (result.Data != null)
                result.Data.User = new UserProfileDto
                {
                    UserId = userId,
                    UserName = name,
                    Email = email,
                    Role = role
                };
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        [EnableRateLimiting(RateLimitingPolicy.DefaultPolicy)]
        public ActionResult GetUsers(CancellationToken ct)
        {
            return Ok(ApiResponse<string>.Ok("Admin Endpoint working", "Users List"));
        }
    }
}
