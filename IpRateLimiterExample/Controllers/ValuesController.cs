using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IpRateLimiter.NET.AltairCA;
using Microsoft.AspNetCore.Mvc;

namespace IpRateLimiterExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [IpRateLimitHttp]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [IpRateLimitHttp(10*60,2,"asd" )]
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
