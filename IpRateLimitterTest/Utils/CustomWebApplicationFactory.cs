using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IpRateLimiter.AspNetCore.AltairCA.Helpers;
using IpRateLimiter.AspNetCore.AltairCA.Interface;
using IpRateLimiter.AspNetCore.AltairCA.Providers;
using IpRateLimiterExample;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IpRateLimitterTest.Utils
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
                services.AddMemoryCache();
                services.AddHttpContextAccessor();
                services.AddScoped<IIpRateLimitStorageProvider, MemoryCacheProvider>();
                services.AddIpRateLimiter(options =>
                {
                    options.GlobalRateLimit = 10;
                    options.GlobalSpan = TimeSpan.FromMinutes(30);
                    options.ExcludeList = new List<string>
                    {
                        "127.0.0.1", "192.168.0.0/24"
                    };
                });
            });
            builder.Configure(app =>
            {
                app.UseMiddleware<FakeRemoteIpAddressMiddleware>();
                app.UseMvc();
            });
        }
    }
}
