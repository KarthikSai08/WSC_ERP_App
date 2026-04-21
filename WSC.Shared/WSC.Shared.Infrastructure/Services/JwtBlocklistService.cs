using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using WSC.Shared.Contracts.Interfaces.JwtInterfaces;

namespace WSC.Shared.Infrastructure.Services
{
    public class JwtBlocklistService : IJwtBlocklistService
    {
        private readonly IDatabase _db;
        private const string Prefix = "jwt:blocked:";
        public JwtBlocklistService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task BlockTokenAsync(string jti, TimeSpan remainingLifeTime)
        {
            await _db.StringSetAsync(
                $"{Prefix}{jti}",
                "blocked",
                remainingLifeTime,
                When.Always);
        }

        public async Task<bool> IsBlockedAsync(string jti)
        {
            return await _db.KeyExistsAsync($"{Prefix}{jti}");
        }
    }
}
