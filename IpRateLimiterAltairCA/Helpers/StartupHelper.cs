using System;
using IpRateLimiter.AspNetCore.AltairCA.Interface;
using IpRateLimiter.AspNetCore.AltairCA.Models;
using IpRateLimiter.AspNetCore.AltairCA.Service;
using Microsoft.Extensions.DependencyInjection;

namespace IpRateLimiter.AspNetCore.AltairCA.Helpers
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
