using System.Collections.Concurrent;

namespace PracticalFuncyness.ConsoleClient.Common.Utils;

internal class MemoizeUtils
{
    private record CacheItem<T>(T CachedValue, DateTimeOffset ExpirationTime);

    public static Func<T1, Task<T2>> Memoize<T1, T2>(Func<T1, Task<T2>> funcToMemoize, int expireInMins = 10) where T1 : notnull
    {
        var cacheManager = new ConcurrentDictionary<T1, CacheItem<T2>>();
        var lockManager = new ConcurrentDictionary<T1, SemaphoreSlim>();

        return async (T1 cacheKey) =>
        {
            bool lockAcquired = false;
            var semaphoreSlim = lockManager.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));

            try
            {
                if (cacheManager.TryGetValue(cacheKey, out var item) && item.ExpirationTime > DateTimeOffset.Now) return item.CachedValue;

                await semaphoreSlim.WaitAsync();
                lockAcquired = true;

                if (cacheManager.TryGetValue(cacheKey, out var cacheItem) && cacheItem.ExpirationTime > DateTimeOffset.Now) return cacheItem.CachedValue;

                var result              = await funcToMemoize(cacheKey);
                var expiration          = DateTimeOffset.Now.AddMinutes(expireInMins);
                cacheManager[cacheKey]  = new CacheItem<T2>(result, expiration);

                return result;
            }
            finally
            {
                if (true == lockAcquired) semaphoreSlim.Release();
            }
        };
    }

    public static Func<T1, T2> Memoize<T1, T2>(Func<T1, T2> funcToMemoize, int expireInMins = 10) where T1 : notnull
    {
        var cacheManager = new ConcurrentDictionary<T1, CacheItem<T2>>();
        var lockManager  = new ConcurrentDictionary<T1, SemaphoreSlim>();

        return (T1 cacheKey) =>
        {
            bool lockAcquired = false;
            var semaphoreSlim = lockManager.GetOrAdd(cacheKey, _ => new SemaphoreSlim(1, 1));

            try
            {
                if (cacheManager.TryGetValue(cacheKey, out var item) && item.ExpirationTime > DateTimeOffset.Now) return item.CachedValue;

                semaphoreSlim.Wait();
                lockAcquired = true;

                if (cacheManager.TryGetValue(cacheKey, out var cacheItem) && cacheItem.ExpirationTime > DateTimeOffset.Now) return cacheItem.CachedValue;

                var result              = funcToMemoize(cacheKey);
                var expiration          = DateTimeOffset.Now.AddMinutes(expireInMins);
                cacheManager[cacheKey]  = new CacheItem<T2>(result, expiration);

                return result;
            }
            finally
            {
                if (true == lockAcquired) semaphoreSlim.Release();
            }
        };
    }

}

