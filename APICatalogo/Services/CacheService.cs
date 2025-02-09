using Microsoft.Extensions.Caching.Memory;

namespace APICatalogo.Services;

public class CacheService
{
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;

        _cacheOptions = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), 
            SlidingExpiration = TimeSpan.FromMinutes(2), 
            Priority = CacheItemPriority.Normal
        };
    }

    public void Set<T>(string key, T value, TimeSpan? expirationTime = null)
    {
        var options = _cacheOptions;
        if (expirationTime.HasValue)
        {
            options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expirationTime.Value,
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Priority = CacheItemPriority.Normal
            };
        }

        _cache.Set(key, value, options);
    }

    public T? TryGetValue<T>(string key)
    {
        return _cache.TryGetValue(key, out T value) ? value : default;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
