using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace RedisCacheDemo.Services.Caching;

public class RedisCacheService(IDistributedCache? cache) : IRedisCacheService
{
    private readonly IDistributedCache? _cache = cache;

    public T? GetData<T>(string key)
    {
        var data = _cache?.GetString(key);

        if(data is null)
            return default(T);

        return JsonSerializer.Deserialize<T>(data);
    }

    public void SetData<T>(string key, T data)
    {
        var option = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        _cache?.SetString(key, JsonSerializer.Serialize(data), option);
    }
}
