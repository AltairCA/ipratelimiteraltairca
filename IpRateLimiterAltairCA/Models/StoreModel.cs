using System;
using System.Collections.Generic;

namespace IpRateLimiterAltairCA
{
    internal class StoreModel
    {
        public string ClientIp { get; set; }
        public string Path { get; set; }
        public List<DateTime> Entries { get; set; }

    }
}
