using System;
using System.Collections.Generic;
using System.Text;
using WSC.Gateway.Domain.Entities;

namespace WSC.Gateway.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expiryDate, CancellationToken ct);
        Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct);
        Task RevokeRefreshTokenAsync(string token, string? replacedByToken, CancellationToken ct);
        Task RevokeAllUserTokenAsync(string userId, CancellationToken ct);
    }
}
