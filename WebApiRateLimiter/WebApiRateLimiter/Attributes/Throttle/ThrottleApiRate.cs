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
                if (!_cache.TryGetValue(_serviceName, out CacheType serviceHitCounter))
                {
                    serviceHitCounter = new CacheType
                    {
                        ExpiresAt = DateTime.Now.AddSeconds(5),
                        Counter = 1
                    };

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
                    if (50 > serviceHitCounter.Counter)
                    {
                        serviceHitCounter.Counter++;
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                        {
                            Priority = CacheItemPriority.High,
                            AbsoluteExpiration = serviceHitCounter.ExpiresAt
                        };
                        _cache.Set(_serviceName, serviceHitCounter, cacheEntryOptions);
                    }
                    else
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                        {
                            Priority = CacheItemPriority.High,
                            AbsoluteExpiration = DateTime.Now.AddSeconds(5)
                        };
                        _cache.Set("ThrottleBaseKey" + _serviceName, cacheEntryOptions);
                    }
                }

                base.OnActionExecuting(context);
            }
        }
    }

    internal class CacheType
    {
        public DateTime ExpiresAt { get; set; }
        public int Counter { get; set; }
    }
}