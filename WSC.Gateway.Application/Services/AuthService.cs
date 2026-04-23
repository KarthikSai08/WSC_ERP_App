using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WSC.Gateway.Application.Dtos.AuthDtos;
using WSC.Gateway.Application.Interfaces;
using WSC.Shared.Contracts.Common;
using System.IdentityModel.Tokens.Jwt;
using WSC.Shared.Contracts.Exceptions;
using WSC.Shared.Contracts.Interfaces.JwtInterfaces;
using System.Linq.Expressions;
using WSC.Gateway.Domain.Entities;

namespace WSC.Gateway.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IJwtService _jwtService;
        private readonly IJwtBlocklistService _blocklistService;
        private readonly ILogger<AuthService> _logger;
        private readonly int _refreshTokenDays;
        private readonly int _accessTokenMinutes;

        public AuthService(IAuthRepository authRepo,
                            IJwtService jwtService,
                            IJwtBlocklistService blocklistService,
                            ILogger<AuthService> logger,
                            IConfiguration config, IRefreshTokenRepository refreshTokenRepo)
        {
            _authRepo = authRepo;
            _jwtService = jwtService;
            _blocklistService = blocklistService;
            _logger = logger;
            _refreshTokenRepo = refreshTokenRepo;
            _refreshTokenDays = int.Parse(config["JwtSettings:RefreshTokenDays"] ?? "7");
            _accessTokenMinutes = int.Parse(config["JwtSettings:AccessTokenMinutes"] ?? "60");
        }
        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken ct)
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);
            var user =await _authRepo.GetUserByEmailAsync(request.Email, ct);

            if (user == null || !user.IsActive)
                throw new InvalidCredentialsException();

            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordValid)
                throw new InvalidCredentialsException();

            var accessToken = _jwtService.GenerateAccessToken(user.UserId, user.UserName, user.Email, user.Role);
            
            var refreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(_refreshTokenDays);

            await _refreshTokenRepo.SaveRefreshTokenAsync(user.UserId, refreshToken, refreshTokenExpiry, ct);
            await _authRepo.UpdateLastLoginAsync(user.UserId, ct);

            _logger.LogInformation("User {Email} logged in successfully", request.Email);
            var response = new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_accessTokenMinutes),
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                UserId = user.UserId
            };
            return ApiResponse<LoginResponseDto>.Ok(response, "Login Successful");
        }

        public async Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken ct)
        {
            var storedToken =await _refreshTokenRepo.GetRefreshTokenAsync(request.RefreshToken, ct);

            if(storedToken == null || storedToken.IsActive)
                throw new UnauthorizedException("Invalid or Expired refresh Token");

            var user =await _authRepo.GetUserByIdAsync(storedToken.UserId, ct);
            if(user == null || !user.IsActive)
                throw new UnauthorizedException("User not found or inactive");

            var newAccessToken = _jwtService.GenerateAccessToken(user.UserId, user.UserName, user.Email, user.Role);

            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(_refreshTokenDays);


            await _refreshTokenRepo.RevokeRefreshTokenAsync(request.RefreshToken, newRefreshToken, ct);
            await _refreshTokenRepo.SaveRefreshTokenAsync(user.UserId, newRefreshToken, refreshTokenExpiry, ct);

            var response = new LoginResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_accessTokenMinutes),
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                UserId = user.UserId
            };
            return ApiResponse<LoginResponseDto>.Ok(response, "Token refreshed successfully");
        }

        public async Task<ApiResponse<bool>> LogoutAsync(string accessToken, string refreshToken, CancellationToken ct)
        {
            try
            {
                var principle = _jwtService.GetPrincipalFromExpiredToken(accessToken);
                if (principle is not null)
                {
                    var jti = principle.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                    var expClaim = principle.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

                    if (!string.IsNullOrEmpty(jti) && long.TryParse(expClaim, out var expUnix))
                    {
                        var expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                        var remaining = expDate - DateTime.UtcNow;

                        if (remaining > TimeSpan.Zero)
                        {
                            await _blocklistService.BlockTokenAsync(jti, remaining);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await _refreshTokenRepo.RevokeRefreshTokenAsync(refreshToken, null, ct);
                }
            return ApiResponse<bool>.Ok(true, "Logout successful");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return ApiResponse<bool>.Failed("Logout failed");
            }
        }

        public async Task<ApiResponse<int>> RegisterAsync(RegisterRequestDto request, CancellationToken ct)
        {
            var exists = await _authRepo.EmailExistsAsync(request.Email, ct);
            if (!exists) 
                throw new DuplicateException("User", request.Email);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new Users
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var userId = await _authRepo.CreateUserAsync(user, ct);

            _logger.LogInformation("New user registered with Id {userId}", userId);
            return ApiResponse<int>.Ok(userId, "Registration successful");
        }
    }
}