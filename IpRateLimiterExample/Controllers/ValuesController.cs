using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IpRateLimiter.AspNetCore.AltairCA;
using IpRateLimiter.AspNetCore.AltairCA.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IpRateLimiterExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IpRateLimitHttp]
    public class ValuesController : ControllerBase
    {
        private readonly IIpRateLimitHttpService _ipRateLimitHttpService;
        private readonly IMemoryCache _memoryCache;
        public ValuesController(IIpRateLimitHttpService ipRateLimitHttpService, IMemoryCache memoryCache)
        {
            _ipRateLimitHttpService = ipRateLimitHttpService;
            _memoryCache = memoryCache;
        }

        // GET api/values
        [HttpGet]
        
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [IpRateLimitHttp(10*60,2,"group1" )]
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpGet("clearlimit")]
        public void RemoveLimit()
        {
            _ipRateLimitHttpService.ClearLimit("group1");
        }
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _ipRateLimitHttpService.ClearLimit("GET:Values/Get");
            await _ipRateLimitHttpService.ClearLimit("group1");
            return Ok();
        }
    }
}
