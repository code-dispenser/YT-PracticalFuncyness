using PracticalFuncyness.ConsoleClient.Common.Functors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalFuncyness.ConsoleClient.Common.Utils;

internal static  class ValidationFunctionCache
{
    private record CacheItem(object CachedValue);

    private static readonly ConcurrentDictionary<string, CacheItem>     _validationFunctions = new();
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> _lockManager         = new();
    public static void AddOrUpdate<T>(string cacheKey, T itemToCache) where T: notnull
    {
        CacheItem cacheItem = new(itemToCache);
        
        var semaphoreSlim = _lockManager.GetOrAdd(cacheKey, new SemaphoreSlim(1, 1));

        semaphoreSlim.Wait();
        
        _validationFunctions.AddOrUpdate(cacheKey, cacheItem, (key, existing) => cacheItem);
        
        semaphoreSlim.Release();

    }

    public static bool TryGet<T>(string cacheKey, string fieldName, string displayName, out Func<T, Validated<T>>? cachedFunction) where T : notnull
    {
        cachedFunction = default;

        if (_validationFunctions.TryGetValue(cacheKey, out var itemInCache) && itemInCache.CachedValue is Func<string, string, Func<T, Validated<T>>> curriedFunction) cachedFunction = curriedFunction(fieldName,displayName);

        return cachedFunction is not null;
    }
}
