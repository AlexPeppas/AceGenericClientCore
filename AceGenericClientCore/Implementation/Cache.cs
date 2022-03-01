using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace AceGenericClientFramework.CacheMechanism
{
    public static class Cache
    {
        private static MemoryCache _cache = new MemoryCache("AceGenericClient");

        private static ConcurrentDictionary<string, object> keyLocks = new ConcurrentDictionary<string, object>();
        private static readonly int defaultCacheLifeTime = 3600; //seconds

        private static void ClearCachedData()
        {
            if (_cache.GetCount() > 0)
                _cache.Dispose();
        }

        public static T GetCached<T>(string cacheKey)
        {
            var cacheValue = (T)_cache.Get(cacheKey);
            return cacheValue;
        }

        public static T UpdateCache<T>(string cacheKey, int cacheLifeTime, T cacheValue)
        {
            lock (keyLocks.GetOrAdd(cacheKey, new object()))
            {
                var prevCachedData = (T)_cache.Get(cacheKey);

                if (prevCachedData != null)
                    _cache.Remove(cacheKey);

                SetCache(cacheKey, cacheValue, cacheLifeTime);

                keyLocks.TryRemove(cacheKey, out object locker);
            }
            return cacheValue;
        }

        private static void SetCache(string cacheKey, object cacheValue, int cacheTime)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = cacheTime < 0
                ? new DateTimeOffset(DateTime.Now.AddSeconds(defaultCacheLifeTime))
                : new DateTimeOffset(DateTime.Now.AddSeconds(cacheTime))
            };

            _cache.Set(cacheKey, cacheValue, cacheItemPolicy);
        }
    }
}
