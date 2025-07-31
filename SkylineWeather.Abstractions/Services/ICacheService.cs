using LanguageExt.Common;

namespace SkylineWeather.Abstractions.Services;

public interface ICacheService
{
    Task<Result<T>> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<Result<T>>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

    Task InvalidateAsync(string key, CancellationToken cancellationToken = default);
}