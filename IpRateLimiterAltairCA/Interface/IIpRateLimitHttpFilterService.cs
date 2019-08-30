using System;
using System.Threading.Tasks;
using IpRateLimiter.NET.AltairCA.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IpRateLimiter.NET.AltairCA.Interface
{
    public interface IIpRateLimitHttpFilterService
    {
        Task<Tuple<bool, IpRateLimitServiceResponse>> Validate(TimeSpan span, int limit, string groupKey);
        Task<Tuple<bool, IpRateLimitServiceResponse>> Validate();
        void SetHeaderAndBodyIfLimitReached(ActionExecutingContext context, IpRateLimitServiceResponse response);
        void SetHeadersIfNotLimitReached(ActionExecutedContext context, IpRateLimitServiceResponse response);
    }
}
