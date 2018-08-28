using Microsoft.Extensions.Caching.Memory;
using System;
using WebApiRateLimiter.Helpers.Interface;

namespace WebApiRateLimiter.Helpers.Providers
{
    public class CacheSettingProvider : ICacheSettingProvider
    {
        public CacheSetting CreateCacheSetting(int expiryInSeconds)
        {
            return new CacheSetting()
            {
                ExpiresAt = DateTime.Now.AddSeconds(expiryInSeconds),
                Value = 1
            };
        }

        public MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheItemPriority cacheItemPriority, DateTime expiryDate)
        {
            return new MemoryCacheEntryOptions()
            {
                Priority = cacheItemPriority,
                AbsoluteExpiration = expiryDate
            };
        }
    }
}