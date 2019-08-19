using System;
using System.Collections.Generic;
using System.Text;
using IpRateLimiterAltairCA.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace IpRateLimiterAltairCA
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
