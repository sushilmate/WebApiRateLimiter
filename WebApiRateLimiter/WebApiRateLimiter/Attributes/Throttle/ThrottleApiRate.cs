using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using WebApiRateLimiter.Helpers.Interface;
using WebApiRateLimiter.Helpers.Providers;

namespace WebApiRateLimiter.Attributes.Throttle
{
    public class ThrottleApiRateAttribute : ActionFilterAttribute
    {
        private readonly object syncLock = new object();
        private readonly string _serviceName;
        private readonly IMemoryCache _cache;
        private readonly IOptions<ApiRateLimitPolicies> _options;
        private readonly ICacheSettingProvider _cacheSettingProvider;
        private readonly ILogger<ThrottleApiRateAttribute> _logger;

        /// <summary>
        /// Throttle Attribute used to limit the web api calls
        /// decorate the web api with ThrottleApiRateAttribute to limit the web api calls
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="cache"></param>
        /// <param name="options"></param>
        /// <param name="cacheSettingProvider"></param>
        /// <param name="logger"></param>
        public ThrottleApiRateAttribute(string serviceName, IMemoryCache cache, IOptions<ApiRateLimitPolicies> options, ICacheSettingProvider cacheSettingProvider, ILogger<ThrottleApiRateAttribute> logger)
        {
            _serviceName = serviceName;
            _cache = cache;
            _options = options;
            _cacheSettingProvider = cacheSettingProvider;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // lock to synchronise the multiple calls on web apis.
            lock (syncLock)
            {
                // read api limit rules from config or by default limit rules
                var apiLimitRuleDetails = _options.Value.Rules.FirstOrDefault(x => x.Endpoint == _serviceName) ?? GetDefaultLimitRateValues();

                // throttle cache is present means service threshold is reached, need to suspend the request.
                if (_cache.Get(GetThrottleBaseKey(_serviceName)) != null)
                {
                    _logger.LogWarning("Request has been suspended for User " + context.HttpContext.User);
                    Suspend(context);
                }
                else
                {
                    if (!_cache.TryGetValue(_serviceName, out CacheSetting serviceHitCounter))
                    {
                        // create service hit counter with 1
                        CreateOrUpdateCache(_serviceName, _cacheSettingProvider.CreateCacheSetting(apiLimitRuleDetails.Period));
                    }
                    else
                    {
                        // as long as threshold is not reached just increment the counter and update the service cache counter
                        if (serviceHitCounter.Value < apiLimitRuleDetails.Limit)
                        {
                            serviceHitCounter.Value++;
                            CreateOrUpdateCache(_serviceName, serviceHitCounter);
                        }
                        else
                        {
                            // api limit threshold is reached, need to add throttle cache in the memory to suspend subsequent calls to api for particular duration
                            CreateOrUpdateCache(GetThrottleBaseKey(_serviceName), _cacheSettingProvider.CreateCacheSetting(apiLimitRuleDetails.SuspendPeriod));
                            _logger.LogWarning("Request has been forbidden for User " + context.HttpContext.User);
                            Forbidden(context);
                        }
                    }
                }
                base.OnActionExecuting(context);
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

        private void Forbidden(ActionExecutingContext actionContext)
        {
            actionContext.Result = new ContentResult()
            {
                Content = Constants.FORBIDDEN_CONTENT
            };
        }

        private void Suspend(ActionExecutingContext actionContext)
        {
            actionContext.Result = new ContentResult()
            {
                Content = Constants.SUSPEND_CONTENT
            };
        }

        private string GetThrottleBaseKey(string serviceName)
        {
            return Constants.THROTTLE_BASE_KEY + serviceName;
        }
    }
}