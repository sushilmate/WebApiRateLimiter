using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiRateLimiter.Attributes.Throttle
{
    public class ThrottleApiRateAttribute : ActionFilterAttribute
    {
        private readonly object syncLock = new object();
        private IMemoryCache _cache;
        private string _serviceName;

        public ThrottleApiRateAttribute(string serviceName, IMemoryCache cache)
        {
            _serviceName = serviceName;
            _cache = cache;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            lock (syncLock)
            {
                //if (!_cache.TryGetValue(, out cacheEntry))
                //{
                //}
            }
            base.OnActionExecuting(context);
        }

        public static class Identifier
        {
            public static readonly string THROTTLE_BASE_IDENTIFIER = "THROTTLE_BASE_";
            public static readonly string THROTTLE_COUNTER_IDENTIFIER = "THROTTLE_COUNT_";
        }
    }
}