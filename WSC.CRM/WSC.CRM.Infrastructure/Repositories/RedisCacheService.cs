using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;
using WSC.CRM.Application.Interfaces;

namespace WSC.CRM.Infrastructure.Repositories
{
    internal sealed class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;
        private readonly ILogger<RedisCacheService> _logger;
        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _db = redis.GetDatabase();
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return default;

            _logger.LogInformation("Cache hit for key: {Key}", key);
            return JsonSerializer.Deserialize<T>(value.ToString());
        }

        public async Task RemoveAsync(string key)
        {
            _logger.LogInformation("Removing cache for key: {Key}", key);
            await _db.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);

            _logger.LogInformation("Setting cache for key: {Key} with expiry: {Expiry}", key, expiry);
            await _db.StringSetAsync(key, json, expiry, When.Always);
        }

    }
}
