using LanguageExt.Common;

namespace SkylineWeather.Abstractions.Services;

public interface ICacheService
{
    Task<Result<T>> GetOrCreateAsync<T>(string key, Func<Task<Result<T>>> factory, TimeSpan? expiration = null);

    Task InvalidateAsync(string key);
}