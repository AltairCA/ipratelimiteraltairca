using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IpRateLimiterAltairCA.Interface
{
    public interface IIpRateLimitStorageProvider
    {
        Task<T> Get<T>(string key);
        Task Set(string key, object obj, TimeSpan span);
        Task Set(string key, object obj);
    }
}
