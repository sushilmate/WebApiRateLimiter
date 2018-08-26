using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;

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
                var serviceHitCounter = 0;
                if (!_cache.TryGetValue(_serviceName, out serviceHitCounter))
                {
                    serviceHitCounter = 1;
                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                    {
                        Priority = CacheItemPriority.High,
                        AbsoluteExpiration = DateTime.Now.AddSeconds(5)
                    };

                    // Save data in cache.
                    _cache.Set(_serviceName, serviceHitCounter, cacheEntryOptions);
                }
                else
                {
                    if(50 > serviceHitCounter)
                    {
                        _cache.Set(_serviceName, serviceHitCounter + 1);
                    }
                }

                base.OnActionExecuting(context);
            }
        }
    }
}