using ScrapingFramework.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ScrapingFramework.Providers.CachingProviders
{
    public class RedisCachingProvider : ICachingProvider, IDisposable
    {
        private ConnectionMultiplexer _redis;

        public RedisCachingProvider(string connectionString = "127.0.0.1")
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
        }

        public async Task CacheItem(string key, string value)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, flags: CommandFlags.FireAndForget);
        }

        public async Task<string> GetValue(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task<bool> HasKey(string key)
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        public async Task RemoveItem(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key, flags: CommandFlags.FireAndForget);
        }

        public void Dispose()
        {
            _redis.Close();
        }
    }
}
