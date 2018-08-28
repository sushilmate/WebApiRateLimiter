using System;

namespace WebApiRateLimiter.Helpers.Providers
{
    public class CacheSetting
    {
        public DateTime ExpiresAt { get; set; }
        public int Value { get; set; }
    }
}