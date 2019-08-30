using System;
using IpRateLimiter.NET.AltairCA.Interface;
using IpRateLimiter.NET.AltairCA.Models;
using IpRateLimiter.NET.AltairCA.Service;
using Microsoft.Extensions.DependencyInjection;

namespace IpRateLimiter.NET.AltairCA.Helpers
{
    public static class StartupHelper
    {
        public static IServiceCollection AddIpRateLimiter(this IServiceCollection service,Action<IpRateLimitOptions> options)
        {
            service.Configure(options);
            service.AddScoped<IIpRateLimitHttpFilterService, IpRateLimitHttpFilterService>();
            return service;
        }
    }
}
