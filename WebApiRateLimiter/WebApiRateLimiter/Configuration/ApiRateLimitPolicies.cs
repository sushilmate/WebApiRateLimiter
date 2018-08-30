using System.Collections.Generic;

namespace WebApiRateLimiter
{
    public class ApiRateLimitPolicies
    {
        public IEnumerable<RateLimitRule> Rules { get; set; }
    }

    public class RateLimitRule
    {
        public string EndPointKey { get; set; }
        public string Endpoint { get; set; }
        public int Period { get; set; }
        public int DefaultPeriod { get; set; }
        public int Limit { get; set; }
        public int DefaultLimit { get; set; }
        public int SuspendPeriod { get; set; }
    }
}