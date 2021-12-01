using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiService.Services;

public static class DistributedCacheExtensions
{
    public static async Task<T?> GetValue<T>(this IDistributedCache cache, string key)
    {
        var bytes = await cache.GetAsync(key);
        return bytes == null ? default : JsonSerializer.Deserialize<T>(bytes);
    }
    
    public static async Task SetValue<T>(this IDistributedCache cache, string key, T value)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
        await cache.SetAsync(key, bytes, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60),
            SlidingExpiration = TimeSpan.FromMinutes(15)
        });
    }
}