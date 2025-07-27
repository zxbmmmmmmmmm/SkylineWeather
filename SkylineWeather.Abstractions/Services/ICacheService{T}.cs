namespace SkylineWeather.Abstractions.Services;

public interface ICacheService
{
    public Task<T> GetCacheAsync<T>(string key);

    public Task CacheAsync<T>(string key, T value);
}