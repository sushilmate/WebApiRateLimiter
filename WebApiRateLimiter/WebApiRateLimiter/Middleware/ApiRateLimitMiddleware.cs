using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApiRateLimiter.Attributes.Throttle;
using WebApiRateLimiter.Helpers.Interface;
using WebApiRateLimiter.Helpers.Providers;

namespace WebApiRateLimiter
{
    public class ApiRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly object _syncLock = new object();
        private readonly IMemoryCache _cache;
        private readonly IOptions<ApiRateLimitPolicies> _options;
        private readonly ICacheSettingProvider _cacheSettingProvider;
        private readonly ILogger<ThrottleApiRateAttribute> _logger;

        public ApiRateLimitMiddleware(RequestDelegate next, IMemoryCache cache, IOptions<ApiRateLimitPolicies> options,
            ICacheSettingProvider cacheSettingProvider, ILogger<ThrottleApiRateAttribute> logger)
        {
            _next = next;
            _cache = cache;
            _options = options;
            _cacheSettingProvider = cacheSettingProvider;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsRequestForbidden(context))
            {
                _logger.LogWarning("Request has been forbidden for User " + context.User);
                await Forbidden(context);
                return;
            }
            await _next.Invoke(context);
        }

        private bool IsRequestForbidden(HttpContext context)
        {
            // lock to synchronise the multiple calls on web apis.
            lock (_syncLock)
            {
                var endPoint = context.Request.Path.Value;
                // read api limit rules from config or by default limit rules
                var apiLimitRuleDetails = _options.Value.Rules.FirstOrDefault(x => endPoint.ToLowerInvariant().Contains(x.Endpoint.ToLowerInvariant()))
                                                                                ?? GetDefaultLimitRateValues();

                // throttle cache is present means service threshold is reached, need to suspend the request.
                if (_cache.Get(GetThrottleBaseKey(endPoint)) != null)
                {
                    return true;
                }
                if (!_cache.TryGetValue(endPoint, out CacheSetting serviceHitCounter))
                {
                    // create service hit counter with 1
                    CreateOrUpdateCache(endPoint, _cacheSettingProvider.CreateCacheSetting(apiLimitRuleDetails.Period));
                    return false;
                }
                // as long as threshold is not reached just increment the counter and update the service cache counter
                if (serviceHitCounter.Value < apiLimitRuleDetails.Limit)
                {
                    serviceHitCounter.Value++;
                    CreateOrUpdateCache(endPoint, serviceHitCounter);
                    return false;
                }
                // api limit threshold is reached, need to add throttle cache in the memory to suspend subsequent calls to api for particular duration
                CreateOrUpdateCache(GetThrottleBaseKey(endPoint), _cacheSettingProvider.CreateCacheSetting(apiLimitRuleDetails.SuspendPeriod));
                return true;
            }
        }

        private RateLimitRule GetDefaultLimitRateValues()
        {
            return new RateLimitRule()
            {
                DefaultLimit = 50,
                Limit = 50,
                DefaultPeriod = 10,
                SuspendPeriod = 10
            };
        }

        private void CreateOrUpdateCache(string cacheName, CacheSetting cacheSetting)
        {
            // Set cache options.
            var cacheEntryOptions = _cacheSettingProvider.CreateMemoryCacheEntryOptions(CacheItemPriority.High, cacheSetting.ExpiresAt);

            // Save data in cache.
            _cache.Set(cacheName, cacheSetting, cacheEntryOptions);
        }

        private Task Forbidden(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return httpContext.Response.WriteAsync(Constants.FORBIDDEN_CONTENT);
        }

        private string GetThrottleBaseKey(string serviceName)
        {
            return Constants.THROTTLE_BASE_KEY + serviceName;
        }
    }

    public static class Extensions
    {
        public static bool ContainsIgnoreCase(this string source, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return source != null && value != null && source.IndexOf(value, stringComparison) >= 0;
        }
    }
}