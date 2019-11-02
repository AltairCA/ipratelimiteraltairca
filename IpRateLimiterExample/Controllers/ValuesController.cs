﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IpRateLimiter.AspNetCore.AltairCA;
using IpRateLimiter.AspNetCore.AltairCA.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IpRateLimiterExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IpRateLimitHttp]
    public class ValuesController : ControllerBase
    {
        private readonly IIpRateLimitHttpService _ipRateLimitHttpService;

        public ValuesController(IIpRateLimitHttpService ipRateLimitHttpService)
        {
            _ipRateLimitHttpService = ipRateLimitHttpService;
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
