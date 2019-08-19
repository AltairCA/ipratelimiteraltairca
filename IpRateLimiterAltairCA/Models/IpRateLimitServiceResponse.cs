using System;
using System.Collections.Generic;
using System.Text;

namespace IpRateLimiterAltairCA.Models
{
    public class IpRateLimitServiceResponse
    {
        public int AvaliableLimit { get; set; }
        public DateTime ResetIn { get; set; }
        public int MaxLimit { get; set; }
        public double Period { get; set; }
    }
}
