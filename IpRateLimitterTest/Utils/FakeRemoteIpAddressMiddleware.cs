using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IpRateLimitterTest.Utils
{
    public class FakeRemoteIpAddressMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IPAddress fakeIpAddress = IPAddress.Parse("127.168.1.32");

        public FakeRemoteIpAddressMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Connection.RemoteIpAddress = fakeIpAddress;

            await this.next(httpContext);
        }
    }
}
