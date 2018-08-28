using Microsoft.Extensions.Caching.Memory;
using System;
using WebApiRateLimiter.Helpers.Providers;

namespace WebApiRateLimiter.Helpers.Interface
{
    public interface ICacheSettingProvider
    {
        CacheSetting CreateCacheSetting(int expiryInSeconds);

        MemoryCacheEntryOptions CreateMemoryCacheEntryOptions(CacheItemPriority cacheItemPriority, DateTime expiryDate);
    }
}