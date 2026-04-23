using System;
using System.Collections.Generic;
using System.Text;
using WSC.Gateway.Application.Dtos.AuthDtos;
using WSC.Shared.Contracts.Common;

namespace WSC.Gateway.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken ct);
        Task<ApiResponse<LoginResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken ct);

        Task<ApiResponse<bool>> LogoutAsync(string accessToken,string refreshToken, CancellationToken ct);
        Task<ApiResponse<int>> RegisterAsync(RegisterRequestDto request, CancellationToken ct);
    }
}
 