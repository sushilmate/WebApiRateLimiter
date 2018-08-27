using System.Collections.Generic;

namespace WebApiRateLimiter
{
    public class ApiRateLimitPolicies
    {
        public IEnumerable<Rule> Rules { get; set; }
    }

    public class Rule
    {
        public string Endpoint { get; set; }
        public int Period { get; set; }
        public int DefaultPeriod { get; set; }
        public int Limit { get; set; }
        public int DefaultLimit { get; set; }
        public int SuspendPeriod { get; set; }
    }
}