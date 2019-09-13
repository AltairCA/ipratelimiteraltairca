using System;
using System.Threading.Tasks;

namespace IpRateLimiter.AspNetCore.AltairCA.Interface
{
    public interface IIpRateLimitStorageProvider
    {
        Task<T> Get<T>(string key);
        Task Set(string key, object obj, TimeSpan span);
        Task Set(string key, object obj);
    }
}
