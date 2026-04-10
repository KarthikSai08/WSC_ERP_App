using StackExchange.Redis;
using System.Text.Json;
using WSC.Delivery.Application.Interfaces;

namespace WSC.Delivery.Infrastructure.Repositories
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;
        public RedisCacheService(IConnectionMultiplexer redis) => _db = redis.GetDatabase();
        public async Task<T?> GetAsync<T>(string key)
        {
            var value =await _db.StringGetAsync(key);

            if(value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value.ToString());

        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            return _db.StringSetAsync(key, json, expiry, When.Always);
        }
    }
}
