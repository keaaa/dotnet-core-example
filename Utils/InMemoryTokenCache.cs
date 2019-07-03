using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace oauthExample.Utils
{
    public class InMemoryTokenCache : TokenCache
    {
        private readonly string _userId;
        private readonly IMemoryCache _memoryCache;

        private string CacheKey => $"TokenCache_{_userId}";
        public InMemoryTokenCache(string userId, IMemoryCache memoryCache)
        {

            BeforeAccess = OnBeforeAccess;
            AfterAccess = OnAfterAccess;
            _userId = userId;
            _memoryCache = memoryCache;
        }

        private void OnAfterAccess(TokenCacheNotificationArgs args)
        {
            if (HasStateChanged)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(14)
                };
                _memoryCache.Set(CacheKey, Serialize(), cacheOptions);
                HasStateChanged = false;
            }
        }

        private void OnBeforeAccess(TokenCacheNotificationArgs args)
        {
            if (_memoryCache.Get(CacheKey) is byte[] userTokenCachePayload)
            {
                Deserialize(userTokenCachePayload);
            }
        }
    }
}