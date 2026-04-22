using Dapper;
using WSC.Gateway.Application.Interfaces;
using WSC.Gateway.Domain.Entities;
using WSC.Gateway.Infrastructure.Persistence;

namespace WSC.Gateway.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DapperContext _context;
        public RefreshTokenRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<RefreshToken?> GetRefreshTokenAsync(int userId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = "SELECT * FROM RefreshTokens WHERE Token = @Token";

            return await con.QueryFirstOrDefaultAsync<RefreshToken>(sql, new { UserId = userId });
        }

        public async Task RevokeAllUserTokenAsync(string userId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = "UPDATE RefreshTokens SET IsRevoked = 1, RevokedAt = SYSUTCDATETIME() WHERE UserId = @UserId AND IsRevoked = 0";
            await con.ExecuteAsync(new CommandDefinition(sql, new { UserId = userId }, cancellationToken: ct));
        }

        public Task RevokeRefreshTokenAsync(string token, string? replacedByToken, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = "UPDATE RefreshTokens SET IsRevoked = 1, RevokedAt = SYSUTCDATETIME(), ReplacedByToken = @ReplacedByToken WHERE Token = @Token ";

            return con.ExecuteAsync(new CommandDefinition(sql, new { Token = token, ReplacedByToken = replacedByToken }, cancellationToken: ct));
        }

        public async Task SaveRefreshTokenAsync(int userId, string refreshToken, DateTime expiryDate, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = "INSERT INTO RefreshTokens (UserId, Token, ExpiresAt) VALUES (@UserId, @Token, @ExpiresAt)";

            await con.ExecuteAsync(new CommandDefinition(sql, new { UserId = userId, Token = refreshToken, ExpiresAt = expiryDate }, cancellationToken: ct));
        }
    }
}
