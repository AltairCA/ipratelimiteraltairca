using System;
using System.Threading.Tasks;
using IpRateLimiter.NET.AltairCA.Interface;
using IpRateLimiter.NET.AltairCA.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace IpRateLimiter.NET.AltairCA
{
    public class IpRateLimitHttpAttribute: Attribute , IAsyncActionFilter
    {
        private TimeSpan _span;
        private int _limit;
        private string _groupKey;
        public IpRateLimitHttpAttribute()
        {
            _span = TimeSpan.MinValue;
            _limit = int.MinValue;
            _groupKey = null;
        }
        public IpRateLimitHttpAttribute(double seconds,int limit,string groupkey = null)
        {

            _span = TimeSpan.FromSeconds(seconds);
            _limit = limit;
            _groupKey = groupkey;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            IIpRateLimitHttpFilterService service = context.HttpContext.RequestServices.GetService<IIpRateLimitHttpFilterService>();
            Tuple<bool, IpRateLimitServiceResponse> res = null;
            if (_span == TimeSpan.MinValue && _limit == int.MinValue && _groupKey == null)
            {
                res = await service.Validate();
            }
            else
            {
                res= await service.Validate(_span,_limit,_groupKey);
            }
            if (res.Item1)
            {
                var resultContext = await next();
                if (res.Item2 != null)
                {
                    service.SetHeadersIfNotLimitReached(resultContext,res.Item2);
                }
            }
            else
            {
                
                service.SetHeaderAndBodyIfLimitReached(context,res.Item2);
            }

        }
    }
}
