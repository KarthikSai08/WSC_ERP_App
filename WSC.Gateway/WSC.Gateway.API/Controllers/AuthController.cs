using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WSC.Gateway.Application.Dtos.AuthDtos;
using WSC.Gateway.Application.Interfaces;
using WSC.Gateway.Domain.Entities;
using WSC.Shared.Contracts.Common;

namespace WSC.Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;
        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> LoginAsync([FromBody] LoginRequestDto dto, CancellationToken ct)
        {
            var result = await _service.LoginAsync(dto, ct);
            if (result.Success)
                return Ok(result);
            return Unauthorized(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> RefreshTokenAsync([FromBody] RefreshTokenRequestDto dto, CancellationToken ct)
        {
            var result = await _service.RefreshTokenAsync(dto, ct);
            if (result.Success)
                return Ok(result);
            return Unauthorized(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<bool>>> LogoutAsync([FromBody] RefreshTokenRequestDto dto, CancellationToken ct)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"]
                .FirstOrDefault()?.Replace("Bearer ", "") ?? string.Empty;

            var result = await _service.LogoutAsync(accessToken, dto.RefreshToken, ct);
            return Ok(result);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<int>>> RegisterAsync([FromBody] RegisterRequestDto dto, CancellationToken ct)
        {
            var result = await _service.RegisterAsync(dto, ct);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<ApiResponse<object>> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(ApiResponse<object>.Ok(new { userId, email, name, role }, "Token Valid"));

        }
    }
}
