using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IpRateLimiterAltairCA.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IpRateLimiterAltairCA.Interface
{
    public interface IIpRateLimitHttpFilterService
    {
        Task<Tuple<bool, IpRateLimitServiceResponse>> Validate(TimeSpan span, int limit, string groupKey);
        Task<Tuple<bool, IpRateLimitServiceResponse>> Validate();
        void SetHeaderAndBodyIfLimitReached(ActionExecutingContext context, IpRateLimitServiceResponse response);
        void SetHeadersIfNotLimitReached(ActionExecutedContext context, IpRateLimitServiceResponse response);
    }
}
