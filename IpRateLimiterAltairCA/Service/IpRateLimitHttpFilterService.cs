using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IpRateLimiter.AspNetCore.AltairCA.Helpers;
using IpRateLimiter.AspNetCore.AltairCA.Interface;
using IpRateLimiter.AspNetCore.AltairCA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace IpRateLimiter.AspNetCore.AltairCA.Service
{
    internal class IpRateLimitHttpFilterService: IIpRateLimitHttpFilterService
    {
        private readonly IIpRateLimitStorageProvider _provider;
        private readonly IHttpContextAccessor _httpContext;
        private IpRateLimitOptions _settings;
        public IpRateLimitHttpFilterService(IIpRateLimitStorageProvider iipRateLimitStorageProvider, IHttpContextAccessor httpContext,IOptions<IpRateLimitOptions> settings)
        {
            _provider = iipRateLimitStorageProvider;
            _httpContext = httpContext;
            _settings = settings.Value;
        }

       

        public async Task<Tuple<bool, IpRateLimitServiceResponse>> Validate()
        {
            return await Validate(_settings.GlobalSpan, _settings.GlobalRateLimit, null);
        }
        public async Task<Tuple<bool, IpRateLimitServiceResponse>> Validate(TimeSpan span, int limit, string groupKey)
        {
            string clientIp = CommonUtils.GetClientIP(_settings,_httpContext);
            if (string.IsNullOrWhiteSpace(clientIp))
            {
                return new Tuple<bool, IpRateLimitServiceResponse>(true,null);
            }

            
            if (_settings.ExcludeList != null)
            {
                IPAddress address = IPAddress.Parse(clientIp);
                foreach (string exculedIps in _settings.ExcludeList)
                {
                    if (exculedIps.Contains("/") && address.IsInSubnet(exculedIps))
                    {
                        return new Tuple<bool, IpRateLimitServiceResponse>(true, null);
                    }
                    if (exculedIps == clientIp)
                    {
                        return new Tuple<bool, IpRateLimitServiceResponse>(true, null);
                    }

                }
            }

            DateTime now = DateTime.UtcNow;
            string path = CommonUtils.GetPath(_httpContext);
            string key = string.Empty;
            if (string.IsNullOrWhiteSpace(groupKey))
            {
                key = CommonUtils.GetKey(clientIp, path);
                
            }
            else
            {
                key = groupKey;
            }
            key = string.Concat(_settings.CachePrefix, key);
            StoreModel model = await _provider.GetAsync<StoreModel>(key);
            if (model == null)
            {
                model = new StoreModel
                {
                    Entries = new List<DateTime>(),
                    Path = path,
                    ClientIp = clientIp
                };
            }
            await Task.Run(() => { model.Entries.RemoveAll(x => x.Add(span) < now); });
            DateTime firstDate = now.Add(span);
            if (model.Entries.Any())
            {
                firstDate = model.Entries.First();
                firstDate = firstDate.Add(span);
            }
            if (model.Entries.Count >= limit)
            {
               
                return new Tuple<bool, IpRateLimitServiceResponse>(false,new IpRateLimitServiceResponse{ResetIn = firstDate,MaxLimit = limit,Period = span.TotalSeconds });
            }
            model.Entries.Add(now);
            await _provider.SetAsync(key, model,span);
            return new Tuple<bool, IpRateLimitServiceResponse>(true, new IpRateLimitServiceResponse{AvaliableLimit = limit  - model.Entries.Count, ResetIn = firstDate, Period = span.TotalSeconds });
        }

        public void SetHeaderAndBodyIfLimitReached(ActionExecutingContext context, IpRateLimitServiceResponse response)
        {
            if (ShouldWriteContent(context.HttpContext, response))
            {
                var jsonString = JsonConvert.SerializeObject(_settings.LimitReachedResponse);
                jsonString = jsonString.Replace("{0}", response.MaxLimit.ToString());
                jsonString = jsonString.Replace("{1}", response.Period.ToString());
                double span = Math.Floor((response.ResetIn - DateTime.UtcNow).TotalSeconds);
                if (span < 0)
                {
                    span = 0;
                }

                jsonString = jsonString.Replace("{2}", span.ToString());
                context.Result = new ContentResult
                {
                    Content = jsonString,
                    ContentType = "application/json",
                    StatusCode = _settings.StatusCode
                };
                SetHttpHeaders(context.HttpContext, response);
            }
           
        }
        public void SetHeadersIfNotLimitReached(ActionExecutedContext context, IpRateLimitServiceResponse response)
        {
            SetHttpHeaders(context.HttpContext, response);
        }

        private void SetHttpHeaders(HttpContext context, IpRateLimitServiceResponse response)
        {
            if (ShouldWriteContent(context, response))
            {
                context.Response.Headers.Remove("x-rate-limit-limit");
                context.Response.Headers.Remove("x-rate-limit-remaining");
                context.Response.Headers.Remove("x-rate-limit-reset");
                context.Response.Headers.Add("x-rate-limit-limit", response.Period.ToString());
                context.Response.Headers.Add("x-rate-limit-remaining", response.AvaliableLimit.ToString());
                context.Response.Headers.Add("x-rate-limit-reset", response.ResetIn.ToString());
            }
            
        }

        private bool ShouldWriteContent(HttpContext context, IpRateLimitServiceResponse response)
        {
            if (context.Response.Headers.ContainsKey("x-rate-limit-remaining"))
            {
                int currentlimit = Convert.ToInt32(context.Response.Headers["x-rate-limit-remaining"].ToString());
                double currentPeriod =
                    Convert.ToDouble(context.Response.Headers["x-rate-limit-limit"].ToString());
                if (currentlimit > response.AvaliableLimit || (currentlimit == response.AvaliableLimit && currentPeriod > response.Period))
                {
                    return true;
                }

                return false;
            }

            return true;
        }
    }
}
