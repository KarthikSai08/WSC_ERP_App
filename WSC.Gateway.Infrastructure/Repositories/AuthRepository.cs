using Dapper;
using System.Data;
using WSC.Gateway.Application.Interfaces;
using WSC.Gateway.Domain.Entities;
using WSC.Gateway.Infrastructure.Persistence;

namespace WSC.Gateway.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DapperContext _context;
        public AuthRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> CreateUserAsync(Users user, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserName", user.UserName);
            parameters.Add("@Email", user.Email);
            parameters.Add("@PasswordHash", user.PasswordHash);
            parameters.Add("@Role", user.Role);
            parameters.Add("@NewUserId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            await con.ExecuteAsync("auth.sp_CreateUser",
                                    parameters,
                                    commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@NewUserId");

        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = "SELECT COUNT(1) FROM auth.Users WHERE Email = @Email AND IsActive = 1";
            var result = await con.QueryFirstOrDefaultAsync<int?>(
                new CommandDefinition(sql, new { Email = email }, cancellationToken: ct));

            return result.HasValue;
        }

        public async Task<Users?> GetUserByEmailAsync(string email, CancellationToken ct)
        {
            using var con = _context.CreateConnection();

            var sql = "SELECT * FROM auth.Users WHERE Email = @Email AND IsActive = 1";

            return await con.QueryFirstOrDefaultAsync<Users>(
                new CommandDefinition(sql, new { Email = email }, cancellationToken: ct));
        }

        public async Task<Users?> GetUserByIdAsync(int userId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = "SELECT * FROM auth.Users WHERE UserId = @UserId AND IsActive = 1";

            return await con.QueryFirstOrDefaultAsync<Users>(
                new CommandDefinition(sql, new { UserId = userId }, cancellationToken: ct));
        }

        public async Task UpdateLastLoginAsync(int userId, CancellationToken ct)
        {
            using var con = _context.CreateConnection();
            var sql = "UPDATE auth.Users SET LastLoginAt = SYSUTCDATETIME() WHERE UserId = @UserId AND IsActive = 1";

            await con.ExecuteAsync(new CommandDefinition(sql, new { UserId = userId }, cancellationToken: ct));
        }
    }
}
