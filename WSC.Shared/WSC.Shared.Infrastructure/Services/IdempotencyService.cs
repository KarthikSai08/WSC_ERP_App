
using StackExchange.Redis;
using WSC.Shared.Contracts.Interfaces;

namespace WSC.Shared.Infrastructure.Services
{
    public class IdempotencyService : IIdempotencyService
    {
        private readonly IDatabase _db;
        public IdempotencyService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task<string?> GetResponseAsync(string key, CancellationToken ct)
        {
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task SetResponseAsync(string key, string response, TimeSpan ttl, CancellationToken ct)
        {
            await _db.StringSetAsync(key, response, ttl, When.Always, CommandFlags.None);
        }
    }
}
