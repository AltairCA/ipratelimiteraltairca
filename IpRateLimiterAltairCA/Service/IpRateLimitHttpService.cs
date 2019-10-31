﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IpRateLimiter.AspNetCore.AltairCA.Interface;
using IpRateLimiter.AspNetCore.AltairCA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace IpRateLimiter.AspNetCore.AltairCA.Service
{
    internal class IpRateLimitHttpService
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

        public async Task RemoveLimit()
        {

        }

        public async Task RemoveLimit(string groupKey)
        {

        }
    }
}
