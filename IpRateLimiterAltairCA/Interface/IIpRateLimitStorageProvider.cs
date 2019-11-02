using System;
using System.Threading.Tasks;

namespace IpRateLimiter.AspNetCore.AltairCA.Interface
{
    public interface IIpRateLimitStorageProvider
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object obj, TimeSpan span);
        Task SetAsync(string key, object obj);
        Task RemoveAsync(string key);
    }
}
