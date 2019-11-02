using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using IpRateLimiter.AspNetCore.AltairCA.Helpers;
using IpRateLimiter.AspNetCore.AltairCA.Interface;
using IpRateLimiter.AspNetCore.AltairCA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace IpRateLimiter.AspNetCore.AltairCA.Service
{
    internal class IpRateLimitHttpService : IIpRateLimitHttpService
    {
        private readonly IIpRateLimitStorageProvider _provider;
        private readonly IHttpContextAccessor _httpContext;
        private IpRateLimitOptions _settings;

        public IpRateLimitHttpService(IIpRateLimitStorageProvider provider, IHttpContextAccessor httpContext, IOptions<IpRateLimitOptions> settings)
        {
            _provider = provider;
            _httpContext = httpContext;
            _settings = settings.Value;
        }

        public async Task ClearLimit()
        {
            string clientIp = CommonUtils.GetClientIP(_settings, _httpContext);
            string path = CommonUtils.GetPath(_httpContext);
            string key = CommonUtils.GetKey(clientIp, path);
            
            await _provider.RemoveAsync(key);
        }

        public async Task ClearLimit([Required(AllowEmptyStrings = false)] string groupKey)
        {
            string clientIp = CommonUtils.GetClientIP(_settings, _httpContext);
            await _provider.RemoveAsync(CommonUtils.GetKey(clientIp, groupKey));
        }
    }
}
