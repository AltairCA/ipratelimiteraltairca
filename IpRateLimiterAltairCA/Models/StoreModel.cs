using System;
using System.Collections.Generic;

namespace IpRateLimiter.AspNetCore.AltairCA.Models
{
    internal class StoreModel
    {
        public string ClientIp { get; set; }
        public string Path { get; set; }
        public List<DateTime> Entries { get; set; }

    }
}
