using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace WebApiRateLimiter.Attributes.Throttle
{
    public class ThrottleApiRateAttribute : ActionFilterAttribute
    {
        private readonly object syncLock = new object();
        private IMemoryCache _cache;
        private IOptions<ApiRateLimitPolicies> _options;
        private string _serviceName;

        public ThrottleApiRateAttribute(string serviceName, IMemoryCache cache, IOptions<ApiRateLimitPolicies> options)
        {
            _serviceName = serviceName;
            _cache = cache;
            _options = options;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            lock (syncLock)
            {
                var apiLimitRuleDetails = _options.Value.Rules.FirstOrDefault(x => x.Endpoint == _serviceName);
                if (apiLimitRuleDetails == null)
                    apiLimitRuleDetails = GetDefaultLimitRateValues();

                if (_cache.Get("ThrottleBaseKey" + _serviceName) == null)
                {
                    if (!_cache.TryGetValue(_serviceName, out CacheType serviceHitCounter))
                    {
                        serviceHitCounter = new CacheType
                        {
                            ExpiresAt = DateTime.Now.AddSeconds(apiLimitRuleDetails.Period),
                            Counter = 1
                        };

                        // Set cache options.
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                        {
                            Priority = CacheItemPriority.High,
                            AbsoluteExpiration = serviceHitCounter.ExpiresAt
                        };

                        // Save data in cache.
                        _cache.Set(_serviceName, serviceHitCounter, cacheEntryOptions);
                    }
                    else
                    {
                        if (serviceHitCounter.Counter < apiLimitRuleDetails.Limit)
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
                                AbsoluteExpiration = DateTime.Now.AddSeconds(apiLimitRuleDetails.SuspendPeriod)
                            };
                            _cache.Set("ThrottleBaseKey" + _serviceName, true, cacheEntryOptions);
                            Forbidden(context);
                        }
                    }
                }
                else
                {
                    Forbidden(context);
                }
                base.OnActionExecuting(context);
            }
        }

        private Rule GetDefaultLimitRateValues()
        {
            return new Rule()
            {
                DefaultLimit = 50,
                DefaultPeriod = 10,
                SuspendPeriod = 10
            };
        }

        private void Forbidden(ActionExecutingContext actionContext)
        {
        }
    }

    internal class CacheType
    {
        public DateTime ExpiresAt { get; set; }
        public int Counter { get; set; }
    }
}