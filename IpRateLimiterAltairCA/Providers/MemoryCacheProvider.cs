using System;
using System.Threading.Tasks;
using IpRateLimiterAltairCA.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace IpRateLimiterAltairCA
{
    public class MemoryCacheProvider: IIpRateLimitStorageProvider
    {
        private IMemoryCache _cache;

        public MemoryCacheProvider(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        public async Task<T> Get<T>(string key)
        {
            
            T model = default;
            await Task.Run(() => { _cache.TryGetValue(key, out model); });

            return model;
            
        }

        public async Task Set(string key, object obj,TimeSpan span)
        {
            await Task.Run(() =>
            {
                _cache.Set(key, obj, span);

            });
        }
        public async Task Set(string key, object obj)
        {
            await Task.Run(() => { _cache.Set(key, obj); });
        }

    }
}
