using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ILogger<ApiRateLimitMiddleware> _logger;

        public ApiRateLimitMiddleware(RequestDelegate next, IMemoryCache cache, IOptions<ApiRateLimitPolicies> options,
            ICacheSettingProvider cacheSettingProvider, ILogger<ApiRateLimitMiddleware> logger)
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
                _logger.LogWarning(LoggingMessages.Forbidden + context.User);
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
                // read api limit rules from config or by default limit rules
                var apiLimitRuleDetails = GetApiLimitRuleDetails(context);

                // throttle cache is present means service threshold is reached, need to suspend the request.
                if (_cache.Get(GetThrottleBaseKey(apiLimitRuleDetails.EndPointKey)) != null)
                {
                    return true;
                }
                if (!_cache.TryGetValue(apiLimitRuleDetails.EndPointKey, out CacheSetting serviceHitCounter))
                {
                    // create service hit counter with 1
                    CreateOrUpdateCache(apiLimitRuleDetails.EndPointKey, _cacheSettingProvider.CreateCacheSetting(apiLimitRuleDetails.Period));
                    return false;
                }
                // as long as threshold is not reached just increment the counter and update the service cache counter
                if (serviceHitCounter.Value < apiLimitRuleDetails.Limit)
                {
                    serviceHitCounter.Value++;
                    CreateOrUpdateCache(apiLimitRuleDetails.EndPointKey, serviceHitCounter);
                    return false;
                }
                // api limit threshold is reached, need to add throttle cache in the memory to suspend subsequent calls to api for particular duration
                CreateOrUpdateCache(GetThrottleBaseKey(apiLimitRuleDetails.EndPointKey), _cacheSettingProvider.CreateCacheSetting(apiLimitRuleDetails.SuspendPeriod));
                return true;
            }
        }

        private RateLimitRule GetApiLimitRuleDetails(HttpContext context)
        {
            var endPoint = context.Request.Path.Value;
            return _options.Value.Rules.FirstOrDefault(x => endPoint.ToLowerInvariant().Contains(x.Endpoint.ToLowerInvariant()))
                                                                                  ?? GetDefaultLimitRateValues(endPoint);
        }

        private static RateLimitRule GetDefaultLimitRateValues(string endPointKey)
        {
            return new RateLimitRule()
            {
                DefaultLimit = 50,
                Limit = 50,
                DefaultPeriod = 10,
                SuspendPeriod = 10,
                EndPointKey = endPointKey
            };
        }

        private void CreateOrUpdateCache(string cacheName, CacheSetting cacheSetting)
        {
            // Set cache options.
            var cacheEntryOptions = _cacheSettingProvider.CreateMemoryCacheEntryOptions(CacheItemPriority.High, cacheSetting.ExpiresAt);

            // Save data in cache.
            _cache.Set(cacheName, cacheSetting, cacheEntryOptions);
        }

        private static Task Forbidden(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return httpContext.Response.WriteAsync(Constants.FORBIDDEN_CONTENT);
        }

        private static string GetThrottleBaseKey(string serviceName)
        {
            return Constants.THROTTLE_BASE_KEY + serviceName;
        }
    }
}