// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.Core;

public class CacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;
    private readonly Dictionary<string, Exception> _exceptions = new();

    /// <summary>
    /// Creates a new cache service.
    /// </summary>
    /// <param name="cache">Cache base layer.</param>
    /// <param name="logger">logger.</param>
    public CacheService(
        IMemoryCache cache,
        ILogger<CacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// Call a method with cache.
    /// </summary>
    /// <typeparam name="T">Response type</typeparam>
    /// <param name="cacheKey">Key</param>
    /// <param name="fallback">Fallback method</param>
    /// <param name="cachedMinutes">Cached minutes. By default, it's 20 minutes. If passed a number less or equal than 0, the cache will not take effect.</param>
    /// <returns>Response</returns>
    public async Task<T> RunWithCache<T>(
        string cacheKey,
        Func<Task<T>> fallback,
        int cachedMinutes = 20)
    {
        if (_exceptions.ContainsKey(cacheKey))
        {
            throw _exceptions[cacheKey];
        }

        if (!_cache.TryGetValue(cacheKey, out T resultValue) || cachedMinutes <= 0)
        {
            try
            {
                resultValue = await fallback();
            }
            catch (Exception e)
            {
                _exceptions.Add(cacheKey, e);
                throw;
            }
            if (cachedMinutes > 0)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(cachedMinutes));

                _cache.Set(cacheKey, resultValue, cacheEntryOptions);
                _logger.LogTrace($"Cache set For {cachedMinutes} minutes! Cached key: {cacheKey}");
            }
        }
        else
        {
            _logger.LogTrace($"Cache hit! Cached key: {cacheKey}");
        }

        return resultValue;
    }
}
