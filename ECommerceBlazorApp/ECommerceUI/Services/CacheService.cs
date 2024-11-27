using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceUI.Services
{
    public class CacheService
    {
        private readonly Dictionary<string, (object Data, DateTime Expiration)> _cache = new Dictionary<string, (object, DateTime)>();
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(10);

        public Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> fetchFunc, TimeSpan? expiration = null)
        {
            if (_cache.ContainsKey(key))
            {
                var (data, expirationTime) = _cache[key];
                if (DateTime.UtcNow < expirationTime)
                {
                    return Task.FromResult((T)data);
                }
                else
                {
                    _cache.Remove(key);
                }
            }

            return AddAsync(key, fetchFunc, expiration);
        }

        private async Task<T> AddAsync<T>(string key, Func<Task<T>> fetchFunc, TimeSpan? expiration = null)
        {
            var data = await fetchFunc();
            _cache[key] = (data, DateTime.UtcNow.Add(expiration ?? _defaultExpiration));
            return data;
        }

        public void Remove(string key)
        {
            if (_cache.ContainsKey(key))
            {
                _cache.Remove(key);
            }
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}
